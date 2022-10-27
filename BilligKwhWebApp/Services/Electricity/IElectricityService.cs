using BilligKwhWebApp.Core.Domain;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace BilligKwhWebApp.Services.Electricity
{
    public partial interface IElectricityService
    {
        IReadOnlyCollection<Schedule> GetSchedulesForDate(DateTime date, int deviceId);
        //Print GetPrintById(string id);
        //void Update(Print print);
        //void Insert(Print print);

        Task UpdateElectricityPrices();
    }
}
