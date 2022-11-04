using BilligKwhWebApp.Services.Interfaces;
using BilligKwhWebApp.Services.Customers;
using System.Collections.Generic;
using BilligKwhWebApp.Services.Electricity.Dto;
using BilligKwhWebApp.Services.SmartDevices.Repository;
using BilligKwhWebApp.Core.Domain;

namespace BilligKwhWebApp.Services.SmartDevices
{
    public class SmartDeviceService : ISmartDeviceService
    {
        private readonly ISystemLogger _logger;
        private readonly ISmartDeviceRepository _arduinoRepository;
        private readonly ICustomerService _customerService;

        public SmartDeviceService(ISystemLogger logger, ISmartDeviceRepository arduinoRepository, ICustomerService customerService)
        {
            _logger = logger;
            _arduinoRepository = arduinoRepository;
            _customerService = customerService;
        }
        public SmartDevice GetSmartDeviceById(int id)
        {
            return _arduinoRepository.GetSmartDeviceById(id);
        }

        public void Update(SmartDevice smartDevice)
        {
            if (smartDevice != null)
                _arduinoRepository.Update(smartDevice);
            else
            {
                _logger.Warning("SmartDevice is NULL in Update!", null, "SmartDeviceService");
            }
        }

        public void Insert(SmartDevice SmartDevice)
        {
            _arduinoRepository.Insert(SmartDevice);
        }

        public IReadOnlyCollection<SmartDeviceDto> GetAllSmartDeviceDto(int customerId)
        {
            return _arduinoRepository.GetAllSmartDeviceDto(customerId);
        }

        public SmartDeviceDto GetSmartDeviceDtoById(int id)
        {
            return _arduinoRepository.GetDtoById(id);
        }

        public SmartDevice GetSmartDeviceByUniqueidentifier(string uniqueidentifier)
        {
            return _arduinoRepository.GetSmartDeviceByUniqueidentifier(uniqueidentifier);
        }
    }
}
