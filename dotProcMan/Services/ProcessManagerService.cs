using dotProcMan.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace dotProcMan.Services
{
    public interface IProcessManagerService
    {
        void Initialize(IEnumerable<ManagedProcess> processes);

        IEnumerable<ManagedProcess> GetProcesses();

        bool Start(Guid processId);

        bool Stop(Guid processId);

        bool Restart(Guid processId);

        bool SendInput(Guid processId, string data);

        string GetName(Guid processId);

        bool IsRunning(Guid processId);
    }

    public class ProcessManagerService : IProcessManagerService, IDisposable
    {
        private class ProcessListItem
        {
            public ManagedProcess Configuration { get; set; }

            public Process Process { get; set; }
        }

        private IProcessOutputService processOutputService;

        private Dictionary<Guid, ProcessListItem> managedProcesses;

        public ProcessManagerService(IProcessOutputService processOutputService)
        {
            this.processOutputService = processOutputService;

            managedProcesses = new Dictionary<Guid, ProcessListItem>();
        }

        public IEnumerable<ManagedProcess> GetProcesses()
        {
            return managedProcesses.Values.Select(v => v.Configuration);
        }

        public void Initialize(IEnumerable<ManagedProcess> processes)
        {
            foreach(ManagedProcess process in processes)
            {
                managedProcesses.Add(process.ID, new ProcessListItem { Configuration = process });

                if (process.StartDelay > 0)
                    Task.Delay(process.StartDelay * 1000).ContinueWith(task => Start(process.ID));
            }
        }

        public bool Start(Guid processId)
        {
            if (!managedProcesses.ContainsKey(processId))
                return false;

            ProcessListItem item = managedProcesses[processId];
            ManagedProcess configuration = item.Configuration;

            if (item.Process != null)
            {
                if (item.Process.HasExited)
                {
                    return false;
                }
            }
            else
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(configuration.FileName);

                if (!string.IsNullOrWhiteSpace(configuration.Args))
                    startInfo.Arguments = configuration.Args;

                if (!string.IsNullOrWhiteSpace(configuration.WorkingDirectory))
                    startInfo.WorkingDirectory = configuration.WorkingDirectory;

                if (!string.IsNullOrWhiteSpace(configuration.Username) && !string.IsNullOrWhiteSpace(configuration.Password))
                {
                    startInfo.UserName = configuration.Username;
                    startInfo.PasswordInClearText = configuration.Password;
                }

                startInfo.CreateNoWindow = true;
                startInfo.LoadUserProfile = false;
                startInfo.UseShellExecute = false;

                startInfo.RedirectStandardError = true;
                startInfo.RedirectStandardInput = true;
                startInfo.RedirectStandardOutput = true;

                Process startedProcess = new Process();
                startedProcess.StartInfo = startInfo;

                startedProcess.OutputDataReceived += (source, ea) => processOutputService.AddStandardOutput(configuration.ID, ea.Data);
                startedProcess.ErrorDataReceived += (source, ea) => processOutputService.AddErrorOutput(configuration.ID, ea.Data);

                startedProcess.EnableRaisingEvents = true;
                startedProcess.Exited += (source, ea) => StartedProcess_Exited(configuration, startedProcess, ea);

                item.Process = startedProcess;
            }

            bool result;
            try
            {
                result = item.Process.Start();
                if (result)
                {
                    item.Process.StandardInput.AutoFlush = true;

                    item.Process.BeginOutputReadLine();
                    item.Process.BeginErrorReadLine();
                }
            }
            catch(Exception ex)
            {
                result = false;
                processOutputService.AddInternalOutput(configuration.ID, $"Process failed to start with exception: {ex.Message} ({ex.GetType().FullName})");
            }

            return result;
        }

        private void StartedProcess_Exited(ManagedProcess process, Process exitedProcess, EventArgs e)
        {
            processOutputService.AddInternalOutput(process.ID, $"Process exited with code: {exitedProcess.ExitCode}");

            if (process.AutoRestart && exitedProcess.ExitCode != 0)
                Task.Delay(1000).ContinueWith(task => Start(process.ID));
        }

        public void Dispose()
        {
            foreach(ProcessListItem item in managedProcesses.Values)
            {
                if(item != null && item.Process != null)
                {
                    item.Process.Kill();
                    item.Process.Dispose();
                }
            }
        }

        public bool SendInput(Guid processId, string data)
        {
            if (!managedProcesses.ContainsKey(processId))
                return false;

            ProcessListItem item = managedProcesses[processId];
            if (item.Process == null || item.Process.HasExited)
                return false;

            // TODO: try/catch?
            item.Process.StandardInput.WriteLine(data);
            return true;
        }

        public string GetName(Guid processId)
        {
            if (!managedProcesses.ContainsKey(processId))
                return null;

            ProcessListItem item = managedProcesses[processId];
            return item.Configuration.Name;
        }

        public bool Stop(Guid processId)
        {
            if (!managedProcesses.ContainsKey(processId))
                return false;

            ProcessListItem item = managedProcesses[processId];
            if (item.Process == null || item.Process.HasExited)
                return false;

            item.Process.Kill();
            return true;
        }

        public bool Restart(Guid processId)
        {
            if (!managedProcesses.ContainsKey(processId))
                return false;

            ProcessListItem item = managedProcesses[processId];

            Stop(processId);
            item.Process.WaitForExit();
            return Start(processId);
        }

        public bool IsRunning(Guid processId)
        {
            if (!managedProcesses.ContainsKey(processId))
                return false;

            if (managedProcesses[processId].Process == null)
                return false;

            return !managedProcesses[processId].Process.HasExited;
        }
    }
}
