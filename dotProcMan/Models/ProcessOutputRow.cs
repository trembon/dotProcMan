using dotProcMan.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotProcMan.Models
{
    public class ProcessOutputRow
    {
        public DateTime Timestamp { get; set; }

        public string Data { get; set; }

        public OutputType Type { get; set; }
    }
}
