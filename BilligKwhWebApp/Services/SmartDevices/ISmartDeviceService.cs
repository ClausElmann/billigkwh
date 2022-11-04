using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Services.Electricity.Dto;
using System.Collections.Generic;
namespace BilligKwhWebApp.Services.SmartDevices
{
    public partial interface ISmartDeviceService
    {
        SmartDevice GetSmartDeviceById(int id);
        SmartDevice GetSmartDeviceByUniqueidentifier(string uniqueidentifier);
        void Update(SmartDevice smartDevice);
        void Insert(SmartDevice smartDevice);
        IReadOnlyCollection<SmartDeviceDto> GetAllSmartDeviceDto(int customerId);
        SmartDeviceDto GetSmartDeviceDtoById(int id);
    }
}
