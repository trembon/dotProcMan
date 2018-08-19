using dotProcMan.Models;
using dotProcMan.Models.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotProcMan.Services
{
    public interface IProcessOutputService
    {
        void AddStandardOutput(Guid id, string data);

        void AddErrorOutput(Guid id, string data);

        void AddInput(Guid id, string data);

        void AddInternalOutput(Guid id, string data);

        IEnumerable<ProcessOutputRow> Get(Guid id, int rows);
    }

    public class ProcessOutputService : IProcessOutputService
    {
        private Dictionary<Guid, List<ProcessOutputRow>> output;
        private ConcurrentDictionary<Guid, object> outputLocks;

        public ProcessOutputService()
        {
            this.output = new Dictionary<Guid, List<ProcessOutputRow>>();
            this.outputLocks = new ConcurrentDictionary<Guid, object>();
        }

        public void AddErrorOutput(Guid id, string data)
        {
            AddOutput(id, OutputType.Error, data);
        }

        public void AddInput(Guid id, string data)
        {
            AddOutput(id, OutputType.Input, data);
        }

        public void AddInternalOutput(Guid id, string data)
        {
            AddOutput(id, OutputType.Internal, data);
        }

        public void AddStandardOutput(Guid id, string data)
        {
            AddOutput(id, OutputType.Standard, data);
        }

        public IEnumerable<ProcessOutputRow> Get(Guid id, int rows)
        {
            object lockObject = outputLocks.GetOrAdd(id, new object());
            lock (lockObject)
            {
                if (!output.ContainsKey(id))
                    return new List<ProcessOutputRow>(0);

                List<ProcessOutputRow> data = output[id];
                if (data.Count > rows)
                    return data.Skip(data.Count - rows);

                return data;
            }
        }

        private void AddOutput(Guid id, OutputType type, string data)
        {
            if (data == null)
                return;

            object lockObject = outputLocks.GetOrAdd(id, new object());
            lock (lockObject)
            {
                if (!output.ContainsKey(id))
                    output.Add(id, new List<ProcessOutputRow>());

                output[id].Add(new ProcessOutputRow
                {
                    Type = type,
                    Data = data,
                    Timestamp = DateTime.Now
                });
            }
        }
    }
}
