using BilligKwhWebApp.Services.Arduino.Domain;
using BilligKwhWebApp.Services.Electricity.Dto;
using System.Collections.Generic;

namespace BilligKwhWebApp.Services.Arduino
{
    public partial interface IArduinoService
    {
        SmartDevice GetSmartDeviceById(string id);
        void Update(SmartDevice smartDevice);
        void Insert(SmartDevice smartDevice);
        IReadOnlyCollection<SmartDeviceDto> GetAllSmartDeviceDto(int kundeId);
        SmartDeviceDto GetSmartDeviceDtoById(int id);
    }
}
