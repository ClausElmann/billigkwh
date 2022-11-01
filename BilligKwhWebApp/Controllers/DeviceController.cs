using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.AspNetCore.Http;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Services.Interfaces;
using BilligKwhWebApp.Models;
using BilligKwhWebApp.Services.Arduino;
using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Services.Electricity;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using BilligKwhWebApp.Core;
using BilligKwhWebApp.Services;
using BilligKwhWebApp.Infrastructure.DataTransferObjects.Common;
using BilligKwhWebApp.Services.Arduino.Dto;

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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PrintDto>))]
        public IActionResult GetPrints(int? countryId, int? customerId)
        {
            customerId ??= WorkContext.CustomerId;

            var prints = _arduinoService.GetAllPrintDto((int)customerId);

            if (prints != null)
            {
                //if (countryId.HasValue)
                //    prints = prints.Where(c => c.c.LandID == countryId.Value).ToList();

                //if (customerId.HasValue)
                //    prints = prints.Where(c => c.KundeID == customerId.Value).ToList();

                return Ok(prints);
            }
            else
            {
                return Ok(new List<PrintDto>());
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDto))]
        [ProducesDefaultResponseType]
        public ActionResult<PrintDto> GetPrint(int id)
        {
            PrintDto dto = _arduinoService.GetDtoById(id);

            if (dto == null) return BadRequest(new { ErrorMessage = "Print not found", WorkContext = WorkContext });

            return Ok(dto);
        }


    }
}