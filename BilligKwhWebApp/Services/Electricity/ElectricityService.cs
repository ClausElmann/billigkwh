using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Services.Interfaces;
using BilligKwhWebApp.Services.Customers;
using BilligKwhWebApp.Services.Arduino;
using BilligKwhWebApp.Services.Arduino.Repository;
using BilligKwhWebApp.Services.Electricity.Repository;
using System.Collections.Generic;
using System;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Jobs.ElectricityPrices;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Linq;

namespace BilligKwhWebApp.Services.Electricity
{
    public class ElectricityService : IElectricityService
    {
        private readonly ISystemLogger _logger;
        private readonly IElectricityRepository _electricityRepository;
        private readonly ICustomerService _customerService;
        private readonly IBaseRepository _baseRepository;

        public ElectricityService(ISystemLogger logger, IElectricityRepository electricityRepository, ICustomerService customerService, IBaseRepository baseRepository)
        {
            _logger = logger;
            _electricityRepository = electricityRepository;
            _customerService = customerService;
            _baseRepository = baseRepository;
        }

        public IReadOnlyCollection<Schedule> GetSchedulesForDate(DateTime date, int deviceId)
        {
            return _electricityRepository.GetSchedulesForDate(date, deviceId);
        }

        public async Task UpdateElectricityPrices()
        {
            _logger.Debug("Hent elpriser starts.");

            HttpClient client = new();
            HttpResponseMessage response = await client.GetAsync($"https://api.energidataservice.dk/dataset/Elspotprices?offset=0&start=2022-01-01T00:00&filter=%7B%22PriceArea%22:%22dk1,dk2%22%7D&sort=HourUTC%20ASC&timezone=dk");
            //HttpResponseMessage response = await client.GetAsync($"https://api.energidataservice.dk/dataset/Elspotprices?offset=0&start={DateTime.UtcNow:yyyy-MM-dd}T00:00&filter=%7B%22PriceArea%22:%22dk1,dk2%22%7D&sort=HourUTC%20ASC&timezone=dk");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var welcome = JsonSerializer.Deserialize<Root>(responseBody);

            List<ElectricityPrice> ElectricityPrices = new();

            DateTime updatedUtc = DateTime.UtcNow;

            foreach (var record in welcome.records.Where(w => w.PriceArea == "DK1"))
            {
                var Dk2 = welcome.records.Where(w => w.HourDK == record.HourDK && w.PriceArea == "DK2").FirstOrDefault();
                ElectricityPrices.Add(new ElectricityPrice()
                {
                    HourUTC = record.HourUTC,
                    HourDK = record.HourDK,
                    HourDKNo = record.HourDK.Hour,
                    HourUTCNo = record.HourUTC.Hour,
                    Dk1 = (decimal)record.SpotPriceDKK / 1000,
                    Dk2 = (decimal)Dk2.SpotPriceDKK / 1000,
                    UpdatedUtc = updatedUtc, 
                });
            }
            _baseRepository.BulkMerge(ElectricityPrices);

            _logger.Debug("Hent elpriser ends, Starting to calculate schedules.");

            DateTime danish = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Romance Standard Time");

            var elpriser = _electricityRepository.GetElectricityPriceForDate(danish.Date);

            var recipes = _electricityRepository.GetRecipes();

            var result = _electricityRepository.Calculate(danish, elpriser, recipes);

            _baseRepository.BulkMerge(result);

            _logger.Debug("Calculate schedules ends");
        }

        //public Print GetPrintById(string printId)
        //{
        //    return _arduinoRepository.GetPrintById(printId);
        //}

        //public void Update(Print print)
        //{
        //    if (print != null)
        //        _arduinoRepository.Update(print);
        //    else
        //    {
        //        _logger.Warning("print is NULL in Update!", null, "ArduinoService");
        //    }
        //}

        //public void Insert(Print print)
        //{
        //    _arduinoRepository.Insert(print);
        //}

    }
}
