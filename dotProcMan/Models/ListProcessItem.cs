using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotProcMan.Models
{
    public class ListProcessItem
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public bool IsRunning { get; set; }
    }
}
