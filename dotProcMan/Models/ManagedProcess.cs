using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotProcMan.Models
{
    public class ManagedProcess
    {
        public Guid ID { get; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public string Args { get; set; }

        public string WorkingDirectory { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public int StartDelay { get; set; }

        public bool AutoRestart { get; set; }

        public ManagedProcess()
        {
            this.ID = Guid.NewGuid();
        }

        public override bool Equals(object obj)
        {
            var process = obj as ManagedProcess;
            return process != null &&
                   ID.Equals(process.ID);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID);
        }

        public override string ToString()
        {
            return $"{Name} ({FileName})";
        }
    }
}
