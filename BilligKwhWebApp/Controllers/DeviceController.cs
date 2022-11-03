using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.AspNetCore.Http;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Services.Interfaces;
using BilligKwhWebApp.Services.Arduino;
using BilligKwhWebApp.Services.Electricity;
using System.Collections.Generic;
using BilligKwhWebApp.Core;
using BilligKwhWebApp.Infrastructure.DataTransferObjects.Common;
using BilligKwhWebApp.Services.Electricity.Dto;

namespace BilligKwhWebApp.Controllers
{
    [Authorize(UserRolePermissionProvider.Bearer)]
    [Route("api/[controller]/[action]")]
    public class DeviceController : BaseController
    {
        private readonly IArduinoService _arduinoService;
        private readonly IElectricityService _electricityService;

        public DeviceController(ISystemLogger logger, IWorkContext workContext, IPermissionService permissionService, IArduinoService arduinoService, IElectricityService electricityService) : base(logger, workContext, permissionService)
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
    }
}