﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotProcMan.Models
{
    public class ListProcessesViewModel
    {
        public IEnumerable<ManagedProcess> Processes { get; set; }
    }
}
