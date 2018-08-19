using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotProcMan.Models;
using dotProcMan.Services;
using Microsoft.AspNetCore.Mvc;

namespace dotProcMan.Controllers
{
    public class ProcessController : Controller
    {
        private IProcessOutputService processOutputService;
        private IProcessManagerService processManagerService;

        public ProcessController(IProcessManagerService processManagerService, IProcessOutputService processOutputService)
        {
            this.processOutputService = processOutputService;
            this.processManagerService = processManagerService;
        }

        public IActionResult Output(Guid id)
        {
            ShowOutputViewModel model = new ShowOutputViewModel();
            model.ProcessID = id;
            model.Rows = processOutputService.Get(id, 100);

            return View(model);
        }

        [HttpPost]
        public IActionResult SendInput(Guid id, string data)
        {
            processManagerService.SendInput(id, data);
            return RedirectToAction("Output", new { id = id });
        }
    }
}