using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Services.Electricity.Dto;
using System;
using System.Collections.Generic;

namespace BilligKwhWebApp.Services.Electricity.Repository
{
    public interface IElectricityRepository
    {
        IReadOnlyCollection<Schedule> Calculate(DateTime danish, IReadOnlyCollection<ElectricityPrice> elpriser, IReadOnlyCollection<SmartDevice> devices);
        IReadOnlyCollection<ElectricityPrice> GetElectricityPricesForDate(DateTime date);

        IReadOnlyCollection<SmartDevice> GetSmartDeviceForRecipes();

        //IReadOnlyCollection<Recipe> GetRecipes();
        IReadOnlyCollection<Schedule> GetSchedulesForDate(DateTime date, int deviceId);

        IReadOnlyCollection<ScheduleDto> GetSchedulesForPeriod(int deviceId, DateTime fromDateUtc, DateTime toDateUtc);

        IReadOnlyCollection<ElectricityPrice> GetElectricityPricesForPeriod(DateTime fromDateUtc, DateTime toDateUtc);

        IReadOnlyCollection<ConsumptionDto> GetConsumptionsPeriod(int deviceId, DateTime fromDateUtc, DateTime toDateUtc);


        //SmartDevice GetSmartDeviceById(string SmartDeviceId);
        Consumption GetConsumptionByIdAndDate(DateTime date, int deviceId);
        void UpdateConsumption(Consumption consumption);
        void InsertConsumption(Consumption consumption);
    }
}
