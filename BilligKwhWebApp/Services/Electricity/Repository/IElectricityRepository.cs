using BilligKwhWebApp.Core.Domain;
using System;
using System.Collections.Generic;

namespace BilligKwhWebApp.Services.Electricity.Repository
{
    public interface IElectricityRepository
    {
        IReadOnlyCollection<Schedule> Calculate(DateTime danish, IReadOnlyCollection<ElectricityPrice> elpriser, IReadOnlyCollection<Recipe> recipes);
        IReadOnlyCollection<ElectricityPrice> GetElectricityPriceForDate(DateTime date);
        IReadOnlyCollection<Recipe> GetRecipes();
        IReadOnlyCollection<Schedule> GetSchedulesForDate(DateTime date, int deviceId);

        //Print GetPrintById(string printId);
        Consumption GetConsumptionByIdAndDate(DateTime date, int deviceId);
        void UpdateConsumption(Consumption consumption);
        void InsertConsumption(Consumption consumption);
    }
}
