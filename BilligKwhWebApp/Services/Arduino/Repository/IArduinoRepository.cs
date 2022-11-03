using BilligKwhWebApp.Services.Arduino.Domain;
using BilligKwhWebApp.Services.Electricity.Dto;
using System.Collections.Generic;

namespace BilligKwhWebApp.Services.Arduino.Repository
{
    public interface IArduinoRepository
    {
        SmartDeviceDto GetDtoById(int id);
        SmartDevice GetSmartDeviceById(string SmartDeviceId);
        void Update(SmartDevice SmartDevice);
        void Insert(SmartDevice SmartDevice);
        IReadOnlyCollection<SmartDeviceDto> GetAllSmartDeviceDto(int kundeId);
    }
}
