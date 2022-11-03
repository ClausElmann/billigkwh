using BilligKwhWebApp.Services.Interfaces;
using BilligKwhWebApp.Services.Customers;
using BilligKwhWebApp.Services.Arduino;
using BilligKwhWebApp.Services.Arduino.Repository;
using System.Collections.Generic;
using BilligKwhWebApp.Services.Arduino.Domain;
using BilligKwhWebApp.Services.Electricity.Dto;

namespace BilligKwhWebApp.Services
{
    public class ArduinoService : IArduinoService
    {
        private readonly ISystemLogger _logger;
        private readonly IArduinoRepository _arduinoRepository;
        private readonly ICustomerService _customerService;

        public ArduinoService(ISystemLogger logger, IArduinoRepository arduinoRepository, ICustomerService customerService)
        {
            _logger = logger;
            _arduinoRepository = arduinoRepository;
            _customerService = customerService;
        }
        public SmartDevice GetSmartDeviceById(string SmartDeviceId)
        {
            return _arduinoRepository.GetSmartDeviceById(SmartDeviceId);
        }

        public void Update(SmartDevice smartDevice)
        {
            if (smartDevice != null)
            {
                _arduinoRepository.Update(smartDevice);
            }
            else
            {
                _logger.Warning("SmartDevice is NULL in Update!", null, "ArduinoService");
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
    }
}
