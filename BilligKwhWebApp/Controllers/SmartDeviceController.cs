using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.AspNetCore.Http;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Services.Interfaces;
using BilligKwhWebApp.Services.Electricity;
using System.Collections.Generic;
using BilligKwhWebApp.Core;
using BilligKwhWebApp.Infrastructure.DataTransferObjects.Common;
using BilligKwhWebApp.Services.Electricity.Dto;
using BilligKwhWebApp.Services.SmartDevices;
using System.Linq;

namespace BilligKwhWebApp.Controllers
{
    [Authorize(UserRolePermissionProvider.Bearer)]
    [Route("api/[controller]/[action]")]
    public class SmartDeviceController : BaseController
    {
        private readonly ISmartDeviceService _arduinoService;
        private readonly IElectricityService _electricityService;

        public SmartDeviceController(ISystemLogger logger, IWorkContext workContext, IPermissionService permissionService, ISmartDeviceService arduinoService, IElectricityService electricityService) : base(logger, workContext, permissionService)
        {
            _arduinoService = arduinoService;
            _electricityService = electricityService;
        }

        [HttpGet, Authorize(UserRolePermissionProvider.Bearer)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SmartDeviceDto>))]
        public IActionResult GetSmartDevices(int? countryId, int? customerId)
        {
            customerId ??= WorkContext.CustomerId;

            var smartDevices = _arduinoService.GetAllSmartDeviceDto((int)customerId);

            if (smartDevices != null)
            {
                //if (countryId.HasValue)
                //    SmartDevices = SmartDevices.Where(c => c.c.LandID == countryId.Value).ToList();

                //if (customerId.HasValue)
                //    SmartDevices = SmartDevices.Where(c => c.CustomerId == customerId.Value).ToList();

                return Ok(smartDevices);
            }
            else
            {
                return Ok(new List<SmartDeviceDto>());
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDto))]
        [ProducesDefaultResponseType]
        public ActionResult<SmartDeviceDto> GetSmartDevice(int id)
        {
            SmartDeviceDto dto = _arduinoService.GetSmartDeviceDtoById(id);

            if (dto == null) return BadRequest(new { ErrorMessage = "SmartDevice not found", WorkContext = WorkContext });

            return Ok(dto);
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDto))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorDto))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult UpdateSmartDevice([FromBody] SmartDeviceDto model)
        {
            if (model == null) return BadRequest("Model is null");

            var entity = _arduinoService.GetSmartDeviceById(model.Id);

            bool recalculate = false;

            if (entity != null)
            {
                if (entity.MaxRate != model.MaxRate) recalculate = true;
                entity.Location = model.Location;
                entity.ZoneId = model.ZoneId;
                entity.MaxRate = model.MaxRate;
                _arduinoService.Update(entity);

                if (recalculate)
                {
                    DateTime danish = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Romance Standard Time");

                    var elpriserIdag = _electricityService.GetElectricityPriceForDate(danish.Date);
                    var elpriserImorgen = _electricityService.GetElectricityPriceForDate(danish.Date.AddDays(1));

                    //TODO GENBEGEN IKKE ALLE!!!!!!!!!!!!!!!!!!
                    var devicesForRecipes = _electricityService.GetSmartDeviceForRecipes();

                    _electricityService.Calculate(danish, elpriserIdag.Concat(elpriserImorgen).ToList(), devicesForRecipes);
                }

                return Ok(entity.Id);
            }
            return BadRequest(new { ErrorMessage = "SmartDevice not found", Model = entity });
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ScheduleDto>))]
        public IActionResult GetSchedulesForPeriod(int deviceId, DateTime fromDateUtc, DateTime toDateUtc)
        {
            var schedules = _electricityService.GetSchedulesForPeriod(deviceId, fromDateUtc, toDateUtc);

            if (schedules != null)
            {
                return Ok(schedules);
            }
            else
            {
                return Ok(new List<ScheduleDto>());
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ScheduleDto>))]
        public IActionResult GetSchedulesForToday(int deviceId)
        {
            DateTime danish = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Romance Standard Time");

            var device = _arduinoService.GetSmartDeviceById(deviceId);

            var today = danish.Date;
            var tomorrow = danish.Date;

            if (danish.Hour > 13)
            {
                tomorrow = tomorrow.AddDays(1).Date;
            }

            var schedules = _electricityService.GetSchedulesForPeriod(deviceId, today, tomorrow);

            if (schedules != null)
            {
                var prises = _electricityService.GetElectricityPricesForPeriod(today, tomorrow.AddDays(1).AddMinutes(-1));

                foreach (var item in schedules)
                {
                    if (device.ZoneId == 1)
                    {
                        item.P00 = prises.FirstOrDefault(p => p.HourDKNo == 0).Dk1;
                        item.P00 = prises.FirstOrDefault(p => p.HourDKNo == 0).Dk1;
                        item.P01 = prises.FirstOrDefault(p => p.HourDKNo == 1).Dk1;
                        item.P02 = prises.FirstOrDefault(p => p.HourDKNo == 2).Dk1;
                        item.P03 = prises.FirstOrDefault(p => p.HourDKNo == 3).Dk1;
                        item.P04 = prises.FirstOrDefault(p => p.HourDKNo == 4).Dk1;
                        item.P05 = prises.FirstOrDefault(p => p.HourDKNo == 5).Dk1;
                        item.P06 = prises.FirstOrDefault(p => p.HourDKNo == 6).Dk1;
                        item.P07 = prises.FirstOrDefault(p => p.HourDKNo == 7).Dk1;
                        item.P08 = prises.FirstOrDefault(p => p.HourDKNo == 8).Dk1;
                        item.P09 = prises.FirstOrDefault(p => p.HourDKNo == 9).Dk1;
                        item.P10 = prises.FirstOrDefault(p => p.HourDKNo == 10).Dk1;
                        item.P11 = prises.FirstOrDefault(p => p.HourDKNo == 11).Dk1;
                        item.P12 = prises.FirstOrDefault(p => p.HourDKNo == 12).Dk1;
                        item.P13 = prises.FirstOrDefault(p => p.HourDKNo == 13).Dk1;
                        item.P14 = prises.FirstOrDefault(p => p.HourDKNo == 14).Dk1;
                        item.P15 = prises.FirstOrDefault(p => p.HourDKNo == 15).Dk1;
                        item.P16 = prises.FirstOrDefault(p => p.HourDKNo == 16).Dk1;
                        item.P17 = prises.FirstOrDefault(p => p.HourDKNo == 17).Dk1;
                        item.P18 = prises.FirstOrDefault(p => p.HourDKNo == 18).Dk1;
                        item.P19 = prises.FirstOrDefault(p => p.HourDKNo == 19).Dk1;
                        item.P20 = prises.FirstOrDefault(p => p.HourDKNo == 20).Dk1;
                        item.P21 = prises.FirstOrDefault(p => p.HourDKNo == 21).Dk1;
                        item.P22 = prises.FirstOrDefault(p => p.HourDKNo == 22).Dk1;
                        item.P23 = prises.FirstOrDefault(p => p.HourDKNo == 23).Dk1;
                    }
                    else if (device.ZoneId == 1)
                    {
                        item.P00 = prises.FirstOrDefault(p => p.HourDKNo == 0).Dk2;
                        item.P00 = prises.FirstOrDefault(p => p.HourDKNo == 0).Dk2;
                        item.P01 = prises.FirstOrDefault(p => p.HourDKNo == 1).Dk2;
                        item.P02 = prises.FirstOrDefault(p => p.HourDKNo == 2).Dk2;
                        item.P03 = prises.FirstOrDefault(p => p.HourDKNo == 3).Dk2;
                        item.P04 = prises.FirstOrDefault(p => p.HourDKNo == 4).Dk2;
                        item.P05 = prises.FirstOrDefault(p => p.HourDKNo == 5).Dk2;
                        item.P06 = prises.FirstOrDefault(p => p.HourDKNo == 6).Dk2;
                        item.P07 = prises.FirstOrDefault(p => p.HourDKNo == 7).Dk2;
                        item.P08 = prises.FirstOrDefault(p => p.HourDKNo == 8).Dk2;
                        item.P09 = prises.FirstOrDefault(p => p.HourDKNo == 9).Dk2;
                        item.P10 = prises.FirstOrDefault(p => p.HourDKNo == 10).Dk2;
                        item.P11 = prises.FirstOrDefault(p => p.HourDKNo == 11).Dk2;
                        item.P12 = prises.FirstOrDefault(p => p.HourDKNo == 12).Dk2;
                        item.P13 = prises.FirstOrDefault(p => p.HourDKNo == 13).Dk2;
                        item.P14 = prises.FirstOrDefault(p => p.HourDKNo == 14).Dk2;
                        item.P15 = prises.FirstOrDefault(p => p.HourDKNo == 15).Dk2;
                        item.P16 = prises.FirstOrDefault(p => p.HourDKNo == 16).Dk2;
                        item.P17 = prises.FirstOrDefault(p => p.HourDKNo == 17).Dk2;
                        item.P18 = prises.FirstOrDefault(p => p.HourDKNo == 18).Dk2;
                        item.P19 = prises.FirstOrDefault(p => p.HourDKNo == 19).Dk2;
                        item.P20 = prises.FirstOrDefault(p => p.HourDKNo == 20).Dk2;
                        item.P21 = prises.FirstOrDefault(p => p.HourDKNo == 21).Dk2;
                        item.P22 = prises.FirstOrDefault(p => p.HourDKNo == 22).Dk2;
                        item.P23 = prises.FirstOrDefault(p => p.HourDKNo == 23).Dk2;
                    }
                }

                return Ok(schedules);
            }
            else
            {
                return Ok(new List<ScheduleDto>());
            }
        }
    }
}