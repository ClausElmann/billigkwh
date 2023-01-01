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
using System.Text.RegularExpressions;
using BilligKwhWebApp.Core.Domain;

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
                //if (entity.MaxRate != model.MaxRate) recalculate = true;
                entity.Location = model.Location;
                entity.ZoneId = model.ZoneId;
                entity.MaxRate = model.MaxRate;
                entity.Comment = model.Comment;
                entity.DisableWeekends = model.DisableWeekends;
                entity.StatusId = model.StatusId;
                entity.MinTemp = model.MinTemp;
                entity.MaxRateAtMinTemp = model.MaxRateAtMinTemp;
                entity.ErrorMail = model.ErrorMail;

                _arduinoService.Update(entity);

                //if (recalculate)
                //{
                    DateTime danish = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Romance Standard Time");

                    var elpriserFraIdag = _electricityService.GetElectricityPriceForDate(danish.Date);

                    _electricityService.Calculate(danish.Date, elpriserFraIdag, new List<SmartDevice> { entity });
                //}

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
                var prices = _electricityService.GetElectricityPricesForPeriod(today, tomorrow.AddDays(1).AddMinutes(-1));

                foreach (var item in schedules)
                {
                    if (!prices.Where(p => p.HourDK.Date == item.Date).Any())
                        continue;

                    if (device.ZoneId == 1)
                    {
                        item.P00 = prices.FirstOrDefault(p => p.HourDKNo == 0 && p.HourDK.Date == item.Date).Dk1;
                        item.P00 = prices.FirstOrDefault(p => p.HourDKNo == 0 && p.HourDK.Date == item.Date).Dk1;
                        item.P01 = prices.FirstOrDefault(p => p.HourDKNo == 1 && p.HourDK.Date == item.Date).Dk1;
                        item.P02 = prices.FirstOrDefault(p => p.HourDKNo == 2 && p.HourDK.Date == item.Date).Dk1;
                        item.P03 = prices.FirstOrDefault(p => p.HourDKNo == 3 && p.HourDK.Date == item.Date).Dk1;
                        item.P04 = prices.FirstOrDefault(p => p.HourDKNo == 4 && p.HourDK.Date == item.Date).Dk1;
                        item.P05 = prices.FirstOrDefault(p => p.HourDKNo == 5 && p.HourDK.Date == item.Date).Dk1;
                        item.P06 = prices.FirstOrDefault(p => p.HourDKNo == 6 && p.HourDK.Date == item.Date).Dk1;
                        item.P07 = prices.FirstOrDefault(p => p.HourDKNo == 7 && p.HourDK.Date == item.Date).Dk1;
                        item.P08 = prices.FirstOrDefault(p => p.HourDKNo == 8 && p.HourDK.Date == item.Date).Dk1;
                        item.P09 = prices.FirstOrDefault(p => p.HourDKNo == 9 && p.HourDK.Date == item.Date).Dk1;
                        item.P10 = prices.FirstOrDefault(p => p.HourDKNo == 10 && p.HourDK.Date == item.Date).Dk1;
                        item.P11 = prices.FirstOrDefault(p => p.HourDKNo == 11 && p.HourDK.Date == item.Date).Dk1;
                        item.P12 = prices.FirstOrDefault(p => p.HourDKNo == 12 && p.HourDK.Date == item.Date).Dk1;
                        item.P13 = prices.FirstOrDefault(p => p.HourDKNo == 13 && p.HourDK.Date == item.Date).Dk1;
                        item.P14 = prices.FirstOrDefault(p => p.HourDKNo == 14 && p.HourDK.Date == item.Date).Dk1;
                        item.P15 = prices.FirstOrDefault(p => p.HourDKNo == 15 && p.HourDK.Date == item.Date).Dk1;
                        item.P16 = prices.FirstOrDefault(p => p.HourDKNo == 16 && p.HourDK.Date == item.Date).Dk1;
                        item.P17 = prices.FirstOrDefault(p => p.HourDKNo == 17 && p.HourDK.Date == item.Date).Dk1;
                        item.P18 = prices.FirstOrDefault(p => p.HourDKNo == 18 && p.HourDK.Date == item.Date).Dk1;
                        item.P19 = prices.FirstOrDefault(p => p.HourDKNo == 19 && p.HourDK.Date == item.Date).Dk1;
                        item.P20 = prices.FirstOrDefault(p => p.HourDKNo == 20 && p.HourDK.Date == item.Date).Dk1;
                        item.P21 = prices.FirstOrDefault(p => p.HourDKNo == 21 && p.HourDK.Date == item.Date).Dk1;
                        item.P22 = prices.FirstOrDefault(p => p.HourDKNo == 22 && p.HourDK.Date == item.Date).Dk1;
                        item.P23 = prices.FirstOrDefault(p => p.HourDKNo == 23 && p.HourDK.Date == item.Date).Dk1;
                    }
                    else if (device.ZoneId == 1)
                    {
                        item.P00 = prices.FirstOrDefault(p => p.HourDKNo == 0 && p.HourDK.Date == item.Date).Dk2;
                        item.P00 = prices.FirstOrDefault(p => p.HourDKNo == 0 && p.HourDK.Date == item.Date).Dk2;
                        item.P01 = prices.FirstOrDefault(p => p.HourDKNo == 1 && p.HourDK.Date == item.Date).Dk2;
                        item.P02 = prices.FirstOrDefault(p => p.HourDKNo == 2 && p.HourDK.Date == item.Date).Dk2;
                        item.P03 = prices.FirstOrDefault(p => p.HourDKNo == 3 && p.HourDK.Date == item.Date).Dk2;
                        item.P04 = prices.FirstOrDefault(p => p.HourDKNo == 4 && p.HourDK.Date == item.Date).Dk2;
                        item.P05 = prices.FirstOrDefault(p => p.HourDKNo == 5 && p.HourDK.Date == item.Date).Dk2;
                        item.P06 = prices.FirstOrDefault(p => p.HourDKNo == 6 && p.HourDK.Date == item.Date).Dk2;
                        item.P07 = prices.FirstOrDefault(p => p.HourDKNo == 7 && p.HourDK.Date == item.Date).Dk2;
                        item.P08 = prices.FirstOrDefault(p => p.HourDKNo == 8 && p.HourDK.Date == item.Date).Dk2;
                        item.P09 = prices.FirstOrDefault(p => p.HourDKNo == 9 && p.HourDK.Date == item.Date).Dk2;
                        item.P10 = prices.FirstOrDefault(p => p.HourDKNo == 10 && p.HourDK.Date == item.Date).Dk2;
                        item.P11 = prices.FirstOrDefault(p => p.HourDKNo == 11 && p.HourDK.Date == item.Date).Dk2;
                        item.P12 = prices.FirstOrDefault(p => p.HourDKNo == 12 && p.HourDK.Date == item.Date).Dk2;
                        item.P13 = prices.FirstOrDefault(p => p.HourDKNo == 13 && p.HourDK.Date == item.Date).Dk2;
                        item.P14 = prices.FirstOrDefault(p => p.HourDKNo == 14 && p.HourDK.Date == item.Date).Dk2;
                        item.P15 = prices.FirstOrDefault(p => p.HourDKNo == 15 && p.HourDK.Date == item.Date).Dk2;
                        item.P16 = prices.FirstOrDefault(p => p.HourDKNo == 16 && p.HourDK.Date == item.Date).Dk2;
                        item.P17 = prices.FirstOrDefault(p => p.HourDKNo == 17 && p.HourDK.Date == item.Date).Dk2;
                        item.P18 = prices.FirstOrDefault(p => p.HourDKNo == 18 && p.HourDK.Date == item.Date).Dk2;
                        item.P19 = prices.FirstOrDefault(p => p.HourDKNo == 19 && p.HourDK.Date == item.Date).Dk2;
                        item.P20 = prices.FirstOrDefault(p => p.HourDKNo == 20 && p.HourDK.Date == item.Date).Dk2;
                        item.P21 = prices.FirstOrDefault(p => p.HourDKNo == 21 && p.HourDK.Date == item.Date).Dk2;
                        item.P22 = prices.FirstOrDefault(p => p.HourDKNo == 22 && p.HourDK.Date == item.Date).Dk2;
                        item.P23 = prices.FirstOrDefault(p => p.HourDKNo == 23 && p.HourDK.Date == item.Date).Dk2;
                    }
                }

                return Ok(schedules);
            }
            else
            {
                return Ok(new List<ScheduleDto>());
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ConsumptionDto>))]
        public IActionResult GetConsumptionsPeriod(int deviceId, DateTime fromDateUtc, DateTime toDateUtc)
        {
            var device = _arduinoService.GetSmartDeviceById(deviceId);

            var consumptions = _electricityService.GetConsumptionsPeriod(deviceId, fromDateUtc, toDateUtc);

            var prises = _electricityService.GetElectricityPricesForPeriod(fromDateUtc, toDateUtc);

            if (consumptions != null)
            {
                foreach (var item in consumptions)
                {
                    if (device.ZoneId == 1)
                    {
                        item.P00 = prises.FirstOrDefault(p => p.HourDKNo == 0 && p.DateDK == item.Date).Dk1;
                        item.P01 = prises.FirstOrDefault(p => p.HourDKNo == 1 && p.DateDK == item.Date).Dk1;
                        item.P02 = prises.FirstOrDefault(p => p.HourDKNo == 2 && p.DateDK == item.Date).Dk1;
                        item.P03 = prises.FirstOrDefault(p => p.HourDKNo == 3 && p.DateDK == item.Date).Dk1;
                        item.P04 = prises.FirstOrDefault(p => p.HourDKNo == 4 && p.DateDK == item.Date).Dk1;
                        item.P05 = prises.FirstOrDefault(p => p.HourDKNo == 5 && p.DateDK == item.Date).Dk1;
                        item.P06 = prises.FirstOrDefault(p => p.HourDKNo == 6 && p.DateDK == item.Date).Dk1;
                        item.P07 = prises.FirstOrDefault(p => p.HourDKNo == 7 && p.DateDK == item.Date).Dk1;
                        item.P08 = prises.FirstOrDefault(p => p.HourDKNo == 8 && p.DateDK == item.Date).Dk1;
                        item.P09 = prises.FirstOrDefault(p => p.HourDKNo == 9 && p.DateDK == item.Date).Dk1;
                        item.P10 = prises.FirstOrDefault(p => p.HourDKNo == 10 && p.DateDK == item.Date).Dk1;
                        item.P11 = prises.FirstOrDefault(p => p.HourDKNo == 11 && p.DateDK == item.Date).Dk1;
                        item.P12 = prises.FirstOrDefault(p => p.HourDKNo == 12 && p.DateDK == item.Date).Dk1;
                        item.P13 = prises.FirstOrDefault(p => p.HourDKNo == 13 && p.DateDK == item.Date).Dk1;
                        item.P14 = prises.FirstOrDefault(p => p.HourDKNo == 14 && p.DateDK == item.Date).Dk1;
                        item.P15 = prises.FirstOrDefault(p => p.HourDKNo == 15 && p.DateDK == item.Date).Dk1;
                        item.P16 = prises.FirstOrDefault(p => p.HourDKNo == 16 && p.DateDK == item.Date).Dk1;
                        item.P17 = prises.FirstOrDefault(p => p.HourDKNo == 17 && p.DateDK == item.Date).Dk1;
                        item.P18 = prises.FirstOrDefault(p => p.HourDKNo == 18 && p.DateDK == item.Date).Dk1;
                        item.P19 = prises.FirstOrDefault(p => p.HourDKNo == 19 && p.DateDK == item.Date).Dk1;
                        item.P20 = prises.FirstOrDefault(p => p.HourDKNo == 20 && p.DateDK == item.Date).Dk1;
                        item.P21 = prises.FirstOrDefault(p => p.HourDKNo == 21 && p.DateDK == item.Date).Dk1;
                        item.P22 = prises.FirstOrDefault(p => p.HourDKNo == 22 && p.DateDK == item.Date).Dk1;
                        item.P23 = prises.FirstOrDefault(p => p.HourDKNo == 23 && p.DateDK == item.Date).Dk1;
                    }
                    else if (device.ZoneId == 1)
                    {
                        item.P00 = prises.FirstOrDefault(p => p.HourDKNo == 0 && p.DateDK == item.Date).Dk2;
                        item.P00 = prises.FirstOrDefault(p => p.HourDKNo == 0 && p.DateDK == item.Date).Dk2;
                        item.P01 = prises.FirstOrDefault(p => p.HourDKNo == 1 && p.DateDK == item.Date).Dk2;
                        item.P02 = prises.FirstOrDefault(p => p.HourDKNo == 2 && p.DateDK == item.Date).Dk2;
                        item.P03 = prises.FirstOrDefault(p => p.HourDKNo == 3 && p.DateDK == item.Date).Dk2;
                        item.P04 = prises.FirstOrDefault(p => p.HourDKNo == 4 && p.DateDK == item.Date).Dk2;
                        item.P05 = prises.FirstOrDefault(p => p.HourDKNo == 5 && p.DateDK == item.Date).Dk2;
                        item.P06 = prises.FirstOrDefault(p => p.HourDKNo == 6 && p.DateDK == item.Date).Dk2;
                        item.P07 = prises.FirstOrDefault(p => p.HourDKNo == 7 && p.DateDK == item.Date).Dk2;
                        item.P08 = prises.FirstOrDefault(p => p.HourDKNo == 8 && p.DateDK == item.Date).Dk2;
                        item.P09 = prises.FirstOrDefault(p => p.HourDKNo == 9 && p.DateDK == item.Date).Dk2;
                        item.P10 = prises.FirstOrDefault(p => p.HourDKNo == 10 && p.DateDK == item.Date).Dk2;
                        item.P11 = prises.FirstOrDefault(p => p.HourDKNo == 11 && p.DateDK == item.Date).Dk2;
                        item.P12 = prises.FirstOrDefault(p => p.HourDKNo == 12 && p.DateDK == item.Date).Dk2;
                        item.P13 = prises.FirstOrDefault(p => p.HourDKNo == 13 && p.DateDK == item.Date).Dk2;
                        item.P14 = prises.FirstOrDefault(p => p.HourDKNo == 14 && p.DateDK == item.Date).Dk2;
                        item.P15 = prises.FirstOrDefault(p => p.HourDKNo == 15 && p.DateDK == item.Date).Dk2;
                        item.P16 = prises.FirstOrDefault(p => p.HourDKNo == 16 && p.DateDK == item.Date).Dk2;
                        item.P17 = prises.FirstOrDefault(p => p.HourDKNo == 17 && p.DateDK == item.Date).Dk2;
                        item.P18 = prises.FirstOrDefault(p => p.HourDKNo == 18 && p.DateDK == item.Date).Dk2;
                        item.P19 = prises.FirstOrDefault(p => p.HourDKNo == 19 && p.DateDK == item.Date).Dk2;
                        item.P20 = prises.FirstOrDefault(p => p.HourDKNo == 20 && p.DateDK == item.Date).Dk2;
                        item.P21 = prises.FirstOrDefault(p => p.HourDKNo == 21 && p.DateDK == item.Date).Dk2;
                        item.P22 = prises.FirstOrDefault(p => p.HourDKNo == 22 && p.DateDK == item.Date).Dk2;
                        item.P23 = prises.FirstOrDefault(p => p.HourDKNo == 23 && p.DateDK == item.Date).Dk2;
                    }
                }

                return Ok(consumptions);
            }
            else
            {
                return Ok(new List<ConsumptionDto>());
            }
        }



        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TemperatureReadingDto>))]
        public IActionResult GetTemperatureReadingsPeriod(int deviceId, DateTime fromDateUtc, DateTime toDateUtc)
        {
            var temperatureReadings = _electricityService.GetTemperatureReadingsPeriod(deviceId, fromDateUtc, toDateUtc.AddDays(1).AddMinutes(-1));

            if (temperatureReadings != null)
            {
                return Ok(temperatureReadings);
            }
            else
            {
                return Ok(new List<TemperatureReadingDto>());
            }
        }

        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RecipeDto>))]
        //public IActionResult GetRecipes(int deviceId)
        //{
        //    var recipes = _electricityService.GetRecipes(deviceId);

        //    if (recipes != null)
        //    {
        //        return Ok(recipes);
        //    }
        //    else
        //    {
        //        return Ok(new List<RecipeDto>());
        //    }
        //}

        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDto))]
        //[ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorDto))]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public IActionResult UpdateRecipe([FromBody] RecipeDto model)
        //{
        //    if (model == null) return BadRequest("Model is null");

        //    var entity = _electricityService.RecipeById(model.Id);

        //    if (entity == null)
        //    {
        //        entity = new Recipe()
        //        {
        //            DeviceId = model.DeviceId,
        //            CustomerId = model.CustomerId,
        //        };
        //    }

        //    entity.Priority = model.Priority;
        //    entity.DayTypeId = model.DayTypeId;
        //    entity.MaxRate = model.MaxRate;
        //    entity.FromHour = model.FromHour;
        //    entity.ToHour = model.ToHour;
        //    entity.MinHours = model.MinHours;
        //    entity.MinTemperature = model.MinTemperature;
        //    entity.MaxRateAtMinTemperature = model.MaxRateAtMinTemperature;

        //    if (entity.Id == 0)
        //    {
        //        _electricityService.InsertRecipe(entity);
        //    }
        //    else
        //    {
        //        _electricityService.UpdateRecipe(entity);
        //    }

        //    var smartdevice = _arduinoService.GetSmartDeviceById(entity.DeviceId);

        //    DateTime danish = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Romance Standard Time");

        //    var elpriserFraIdag = _electricityService.GetElectricityPriceForDate(danish.Date);

        //    _electricityService.Calculate(danish, elpriserFraIdag, new List<SmartDevice> { smartdevice });

        //    return Ok(entity.Id);
        //}
    }
}