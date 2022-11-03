using BilligKwhWebApp.Core.Domain;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using BilligKwhWebApp.Services.Electricity.Dto;

namespace BilligKwhWebApp.Services.Electricity
{
    public partial interface IElectricityService
    {
        IReadOnlyCollection<Schedule> GetSchedulesForDate(DateTime date, int deviceId);
        IReadOnlyCollection<ScheduleDto> GetSchedulesForPeriod(int deviceId, DateTime fromDateUtc, DateTime toDateUtc);
        //SmartDevice GetSmartDeviceById(string id);
        //void Update(SmartDevice SmartDevice);
        //void Insert(SmartDevice SmartDevice);

        Task UpdateElectricityPrices();
        void UpdateConsumption(int deviceId, IReadOnlyCollection<long> list);
    }
}
