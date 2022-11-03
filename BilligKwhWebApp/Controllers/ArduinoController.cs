using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.AspNetCore.Http;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Services.Interfaces;
using BilligKwhWebApp.Models;
using BilligKwhWebApp.Services.Arduino;
using BilligKwhWebApp.Services.Electricity;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using BilligKwhWebApp.Services.Arduino.Domain;

namespace BilligKwhWebApp.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    public class ArduinoController : BaseController
    {
        private readonly IArduinoService _arduinoService;
        private readonly IElectricityService _electricityService;

        public ArduinoController(ISystemLogger logger, IWorkContext workContext, IPermissionService permissionService, IArduinoService arduinoService, IElectricityService electricityService) : base(logger, workContext, permissionService)
        {
            _arduinoService = arduinoService;
            _electricityService = electricityService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BilligKwhModel))]
        public IActionResult GetBilligKwhModel(string deviceId, string consumption)
        {
            var smartDevice = _arduinoService.GetSmartDeviceById(deviceId);

            if (smartDevice == null)
            {
                smartDevice = new SmartDevice()
                {
                    Uniqueidentifier = deviceId,
                    CreatedUtc = DateTime.UtcNow,
                    LatestContactUtc = DateTime.UtcNow,
                    Comment = "",
                    Location = "",
                    ZoneId = 1,
                    MaxRate = 2,
                };
                _arduinoService.Insert(smartDevice);
            }
            else
            {
                smartDevice.LatestContactUtc = DateTime.UtcNow;
                _arduinoService.Update(smartDevice);

                if (!string.IsNullOrEmpty(consumption))
                {
                    var numbers = consumption?.Split(',')?.Select(long.Parse)?.ToList();
                    _electricityService.UpdateConsumption(smartDevice.Id, numbers);
                }
            }

            DateTime danish = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Romance Standard Time");

            var schedules = _electricityService.GetSchedulesForDate(danish.Date, smartDevice.Id);

            if (!schedules.Any())
                return NotFound("Schedules not found"); ;

            List<long> list = new();


            //long[] recipe = new long[schedules.Count * 25];

            foreach (var item in schedules)
            {
                list.Add(long.Parse(item.Date.ToString("yyMMdd")));
                list.Add(item.H00 ? 1 : 0);
                list.Add(item.H01 ? 1 : 0);
                list.Add(item.H02 ? 1 : 0);
                list.Add(item.H03 ? 1 : 0);
                list.Add(item.H04 ? 1 : 0);
                list.Add(item.H05 ? 1 : 0);
                list.Add(item.H06 ? 1 : 0);
                list.Add(item.H07 ? 1 : 0);
                list.Add(item.H08 ? 1 : 0);
                list.Add(item.H09 ? 1 : 0);
                list.Add(item.H10 ? 1 : 0);
                list.Add(item.H11 ? 1 : 0);
                list.Add(item.H12 ? 1 : 0);
                list.Add(item.H13 ? 1 : 0);
                list.Add(item.H14 ? 1 : 0);
                list.Add(item.H15 ? 1 : 0);
                list.Add(item.H16 ? 1 : 0);
                list.Add(item.H17 ? 1 : 0);
                list.Add(item.H18 ? 1 : 0);
                list.Add(item.H19 ? 1 : 0);
                list.Add(item.H20 ? 1 : 0);
                list.Add(item.H21 ? 1 : 0);
                list.Add(item.H22 ? 1 : 0);
                list.Add(item.H23 ? 1 : 0);
            }


            //long[] myNum = { 221020, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 221021, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, };
            //long[] myNum1 = { 221020, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 221021, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };

            var datetime = DateTime.Now;

            var model = new BilligKwhModel()
            {
                Year = int.Parse(datetime.ToString("yy")),
                Month = datetime.Month,
                Day = datetime.Day,
                Hour = datetime.Hour,
                Minute = datetime.Minute,
                Second = datetime.Second,
                Recipe = list.ToArray(),
                //Recipe = DateTime.Now.Millisecond % 2 == 0 ? myNum : myNum1,
                DeviceID = deviceId
            };
            return Ok(model);
        }

        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
        //public IActionResult OpretSmartDevice()
        //{
        //    SmartDevice entity = new()
        //    {
        //        SmartDeviceId = Guid.NewGuid(),
        //        OprettetDatoUtc = DateTime.UtcNow,
        //        SidsteKontaktDatoUtc = DateTime.UtcNow,
        //    };

        //    _arduinoService.Insert(entity);

        //    return Ok(entity.SmartDeviceId);
        //}

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> UpdateElectricityPrices()
        {
            await _electricityService.UpdateElectricityPrices();
            return Ok();
        }






    }
}