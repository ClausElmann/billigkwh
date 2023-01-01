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

        IReadOnlyCollection<Schedule> GetSchedulesForDate(DateTime date, int deviceId);

        IReadOnlyCollection<ScheduleDto> GetSchedulesForPeriod(int deviceId, DateTime fromDateUtc, DateTime toDateUtc);

        IReadOnlyCollection<ElectricityPrice> GetElectricityPricesForPeriod(DateTime fromDateUtc, DateTime toDateUtc);

        IReadOnlyCollection<ConsumptionDto> GetConsumptionsPeriod(int deviceId, DateTime fromDateUtc, DateTime toDateUtc);

        //SmartDevice GetSmartDeviceById(string SmartDeviceId);
        Consumption GetConsumptionByIdAndDate(DateTime date, int deviceId);
        void UpdateConsumption(Consumption consumption);
        void InsertConsumption(Consumption consumption);
        ElectricityPrice GetLatestElectricityPrice();
        IReadOnlyCollection<SmartDevice> GetNoContactToDevices(DateTime datetimeUtc);
        void InsertTemperatureReading(TemperatureReading entity);
        IReadOnlyCollection<TemperatureReadingDto> GetTemperatureReadingsPeriod(int deviceId, DateTime fromDateUtc, DateTime toDateUtc);
        //IReadOnlyCollection<RecipeDto> GetRecipes(int deviceId);
        //Recipe RecipeById(int id);

        //void InsertRecipe(Recipe entity);
        //void UpdateRecipe(Recipe entity);

        //IReadOnlyCollection<KwhConsumption> GetKwhConsumptionsByDeviceIdAndDate(int deviceId, DateTime date);

        
    }
}
