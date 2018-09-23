using dotProcMan.Services;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotProcMan.ScheduledJobs
{
    public class RestartProcessScheduledJob : IJob
    {
        private IProcessOutputService processOutputService;
        private IProcessManagerService processManagerService;

        public RestartProcessScheduledJob(IProcessManagerService processManagerService, IProcessOutputService processOutputService)
        {
            this.processOutputService = processOutputService;
            this.processManagerService = processManagerService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            Guid processId = Guid.Empty;
            try
            {
                processId = Guid.Parse(context.JobDetail.Key.Name);
                if (processManagerService.IsRunning(processId))
                {
                    bool result = processManagerService.Restart(processId);

                    if (result)
                        processOutputService.AddInternalOutput(processId, $"Process restarted with schedule.");
                }
            }
            catch (Exception ex)
            {
                processOutputService.AddInternalOutput(processId, $"Failed to restart process with schedule. ({ex.GetType().Name}: {ex.Message})");
            }

            return Task.CompletedTask;
        }
    }
}
