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
        IReadOnlyCollection<SmartDevice> GetSmartDeviceForRecipes();

        IReadOnlyCollection<ElectricityPrice> GetElectricityPriceForDate(DateTime date);
        IReadOnlyCollection<ElectricityPrice> GetElectricityPricesForPeriod(DateTime fromDateUtc, DateTime toDateUtc);


        IReadOnlyCollection<ConsumptionDto> GetConsumptionsPeriod(int deviceId, DateTime fromDateUtc, DateTime toDateUtc);


        //IReadOnlyCollection<Recipe> GetRecipes();
        void Calculate(DateTime danish, IReadOnlyCollection<ElectricityPrice> elpriser, IReadOnlyCollection<SmartDevice> devices);

        //SmartDevice GetSmartDeviceById(string id);
        //void Update(SmartDevice SmartDevice);
        //void Insert(SmartDevice SmartDevice);

        Task UpdateElectricityPrices();
        void UpdateConsumption(int deviceId, IReadOnlyCollection<long> list);

        void SendNoContactToDeviceAdvices();

        IReadOnlyCollection<TemperatureReadingDto> GetTemperatureReadingsPeriod(int deviceId, DateTime fromDateUtc, DateTime toDateUtc);
    }
}
