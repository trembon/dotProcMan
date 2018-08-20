using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dotProcMan.Models;
using dotProcMan.Services;

namespace dotProcMan.Controllers
{
    public class HomeController : Controller
    {
        private IProcessManagerService processManagerService;

        public HomeController(IProcessManagerService processManagerService)
        {
            this.processManagerService = processManagerService;
        }

        public IActionResult Index()
        {
            ListProcessesViewModel model = new ListProcessesViewModel();
            model.Processes = processManagerService.GetProcesses();

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
