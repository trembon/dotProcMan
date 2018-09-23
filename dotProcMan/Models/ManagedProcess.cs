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

        public string RestartSchedule { get; set; }

        public ManagedProcess()
        {
            this.ID = Guid.NewGuid();
        }

        #region Overrides
        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var process = obj as ManagedProcess;
            return process != null &&
                   ID.Equals(process.ID);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(ID);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{Name} ({FileName})";
        }
        #endregion
    }
}
