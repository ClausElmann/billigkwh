using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.AspNetCore.Http;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Services.Interfaces;
using BilligKwhWebApp.Models;
using BilligKwhWebApp.Services.Electricity;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Services.SmartDevices;
using System.Text.Json;
using BilligKwhWebApp.Services;

namespace BilligKwhWebApp.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    public class AController : BaseController
    {
        private readonly ISmartDeviceService _smartDeviceService;
        private readonly IElectricityService _electricityService;
        //private readonly ISystemLogger _Logger;

        public AController(ISystemLogger logger, IWorkContext workContext, IPermissionService permissionService, ISmartDeviceService smartDeviceService, IElectricityService electricityService) : base(logger, workContext, permissionService)
        {
            _smartDeviceService = smartDeviceService;
            _electricityService = electricityService;
            //_Logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BilligKwhModel))]
        public IActionResult G(string d, string c)
        {
            var smartDevice = _smartDeviceService.GetSmartDeviceByUniqueidentifier(d);

            if (smartDevice == null)
            {
                smartDevice = new SmartDevice()
                {
                    Uniqueidentifier = d,
                    CreatedUtc = DateTime.UtcNow,
                    LatestContactUtc = DateTime.UtcNow,
                    Comment = "",
                    Location = "",
                    ZoneId = 1,
                    MaxRate = 2,
                    Delay = 0,
                    DebugMinutes = 0,
                };
                _smartDeviceService.Insert(smartDevice);
            }
            else
            {
                smartDevice.LatestContactUtc = DateTime.UtcNow;
                _smartDeviceService.Update(smartDevice);

                if (!string.IsNullOrEmpty(c))
                {
                    var numbers = c?.Split(',')?.Select(long.Parse)?.ToList();
                    _electricityService.UpdateConsumption(smartDevice.Id, numbers);
                }
            }

            DateTime danish = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Romance Standard Time");

            var schedules = _electricityService.GetSchedulesForDate(danish.Date, smartDevice.Id);

            if (!schedules.Any())
            {
                var now = DateTime.Now;

                long[] emptyRecipe = { 220101, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                var emptyModel = new BilligKwhModel()
                {
                    Y = int.Parse(now.ToString("yy")),
                    Mo = now.Month,
                    D = now.Day,
                    H = now.Hour,
                    M = now.Minute,
                    S = now.Second,
                    R = emptyRecipe.ToArray(),
                    //DeviceID = smartDevice.Uniqueidentifier,
                    De = smartDevice.Delay,
                    //DebugMinutes = smartDevice.DebugMinutes,
                };

                return Ok(emptyModel);
            }
            //    return NotFound("Schedules not found"); ;

            List<long> list = new();

            //long[] recipe = new long[schedules.Count * 25];

            foreach (var item in schedules)
            {
                list.Add(long.Parse(item.Date.ToString("yyMMdd")));
                list.Add(item.H00);
                list.Add(item.H01);
                list.Add(item.H02);
                list.Add(item.H03);
                list.Add(item.H04);
                list.Add(item.H05);
                list.Add(item.H06);
                list.Add(item.H07);
                list.Add(item.H08);
                list.Add(item.H09);
                list.Add(item.H10);
                list.Add(item.H11);
                list.Add(item.H12);
                list.Add(item.H13);
                list.Add(item.H14);
                list.Add(item.H15);
                list.Add(item.H16);
                list.Add(item.H17);
                list.Add(item.H18);
                list.Add(item.H19);
                list.Add(item.H20);
                list.Add(item.H21);
                list.Add(item.H22);
                list.Add(item.H23);
            }
            //long[] myNum = { 221020, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 221021, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, };
            //long[] myNum1 = { 221020, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 221021, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 };

            //var datetime = DateTime.Now;

            if (smartDevice.DebugMinutes > 0)
            {
                danish = danish.AddMinutes(Convert.ToDouble(smartDevice.DebugMinutes));
            }

            var model = new BilligKwhModel()
            {
                Y = int.Parse(danish.ToString("yy")),
                Mo = danish.Month,
                D = danish.Day,
                H = danish.Hour,
                M = danish.Minute,
                S = danish.Second,
                R = list.ToArray(),
                //Recipe = DateTime.Now.Millisecond % 2 == 0 ? myNum : myNum1,
                //DeviceID = smartDevice.Uniqueidentifier,
                De = smartDevice.Delay,
                //DebugMinutes = smartDevice.DebugMinutes,
            };

            string json = JsonSerializer.Serialize(model);

            return Ok(json);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> UpdateElectricityPrices()
        {
            await _electricityService.UpdateElectricityPrices();
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult SendNoContactToDeviceAdvices()
        {
            _electricityService.SendNoContactToDeviceAdvices();
            return Ok();
        }
    }
}