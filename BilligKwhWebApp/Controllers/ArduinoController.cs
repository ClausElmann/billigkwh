using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.AspNetCore.Http;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Services.Interfaces;
using BilligKwhWebApp.Models;

namespace BilligKwhWebApp.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    public class ArduinoController : BaseController
    {
        public ArduinoController(ISystemLogger logger, IWorkContext workContext, IPermissionService permissionService) : base(logger, workContext, permissionService)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BilligKwhModel))]
        public IActionResult GetBilligKwhModel(string deviceId)
        {
            bool[] myNum = { true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false };

            var model = new BilligKwhModel()
            {
                ServerTid = DateTime.UtcNow,
                Today = myNum,
                Tomorrow = myNum,
                DeviceID = deviceId
            };
            return Ok(model);
        }
    }
}