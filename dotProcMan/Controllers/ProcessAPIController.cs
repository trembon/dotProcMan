using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotProcMan.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dotProcMan.Controllers
{
    [Route("api/process")]
    [ApiController]
    public class ProcessAPIController : ControllerBase
    {
        private IProcessManagerService processManagerService;

        public ProcessAPIController(IProcessManagerService processManagerService)
        {
            this.processManagerService = processManagerService;
        }

        [HttpPost]
        [Route("start")]
        public IActionResult Start(Guid id)
        {
            bool result = processManagerService.Start(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("stop")]
        public IActionResult Stop(Guid id)
        {
            bool result = processManagerService.Stop(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("restart")]
        public IActionResult Restart(Guid id)
        {
            bool result = processManagerService.Restart(id);
            return Ok(result);
        }
    }
}