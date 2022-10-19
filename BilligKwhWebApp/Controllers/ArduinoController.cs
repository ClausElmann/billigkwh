using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.AspNetCore.Http;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Services.Interfaces;
using BilligKwhWebApp.Models;
using BilligKwhWebApp.Services.Localization;
using BilligKwhWebApp.Services;
using Microsoft.AspNetCore.Hosting;
using BilligKwhWebApp.Services.Arduino;
using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Dto;
using BilligKwhWebApp.Core;

namespace BilligKwhWebApp.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    public class ArduinoController : BaseController
    {
        private readonly IArduinoService _arduinoService;

        public ArduinoController(ISystemLogger logger, IWorkContext workContext, IPermissionService permissionService, IArduinoService arduinoService) : base(logger, workContext, permissionService)
        {
            _arduinoService = arduinoService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BilligKwhModel))]
        public IActionResult GetBilligKwhModel(string deviceId)
        {
            var print = _arduinoService.GetPrintById(deviceId);

            if (print == null)
            {
                print = new Print()
                {
                    PrintId = deviceId,
                    OprettetDatoUtc = DateTime.UtcNow,
                    SidsteKontaktDatoUtc = DateTime.UtcNow,
                };
                _arduinoService.Insert(print);
            }
            else
            {
                print.SidsteKontaktDatoUtc = DateTime.UtcNow;
                _arduinoService.Update(print);
            }

            bool[] myNum = { true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false };

            var datetime = DateTime.Now;

            var model = new BilligKwhModel()
            {
                Year = int.Parse(datetime.ToString("yy")),
                Month = datetime.Month,
                Day = datetime.Day,
                Hour = datetime.Hour,
                Minute = datetime.Minute,
                Second = datetime.Second,
                Today = myNum,
                Tomorrow = myNum,
                DeviceID = deviceId
            };
            return Ok(model);
        }

        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
        //public IActionResult OpretPrint()
        //{
        //    Print entity = new()
        //    {
        //        PrintId = Guid.NewGuid(),
        //        OprettetDatoUtc = DateTime.UtcNow,
        //        SidsteKontaktDatoUtc = DateTime.UtcNow,
        //    };

        //    _arduinoService.Insert(entity);

        //    return Ok(entity.PrintId);
        //}
    }
}