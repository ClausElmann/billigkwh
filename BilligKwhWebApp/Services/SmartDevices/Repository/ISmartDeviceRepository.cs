using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Services.Electricity.Dto;
using System.Collections.Generic;

namespace BilligKwhWebApp.Services.SmartDevices.Repository
{
    public interface ISmartDeviceRepository
    {
        SmartDeviceDto GetDtoById(int id);
        SmartDevice GetSmartDeviceById(int id);
        SmartDevice GetSmartDeviceByUniqueidentifier(string uniqueidentifier);
        void Update(SmartDevice SmartDevice);
        void Insert(SmartDevice SmartDevice);
        IReadOnlyCollection<SmartDeviceDto> GetAllSmartDeviceDto(int customerId);
    }
}
