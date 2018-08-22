using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotProcMan.Models
{
    public class ShowOutputViewModel
    {
        public Guid ProcessID { get; set; }

        public string ProcessName { get; set; }

        public IEnumerable<ProcessOutputRow> Rows { get; set; }
    }
}
