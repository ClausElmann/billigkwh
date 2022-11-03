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
using Microsoft.AspNetCore.Http;
using System.Globalization;
using BilligKwhWebApp.Core.Toolbox;
using BilligKwhWebApp.Services.Electricity.Dto;

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
            //HttpResponseMessage response = await client.GetAsync($"https://api.energidataservice.dk/dataset/Elspotprices?offset=0&start=2022-01-01T00:00&filter=%7B%22PriceArea%22:%22dk1,dk2%22%7D&sort=HourUTC%20ASC&timezone=dk");
            HttpResponseMessage response = await client.GetAsync($"https://api.energidataservice.dk/dataset/Elspotprices?offset=0&start={DateTime.UtcNow.AddDays(-2):yyyy-MM-dd}T00:00&filter=%7B%22PriceArea%22:%22dk1,dk2%22%7D&sort=HourUTC%20ASC&timezone=dk");
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
            _baseRepository.BulkMerge(ElectricityPrices.DistinctBy(d => d.HourDK));

            _logger.Debug("Hent elpriser ends, Starting to calculate schedules.");

            DateTime danish = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Romance Standard Time");

            var elpriser = _electricityRepository.GetElectricityPriceForDate(danish.Date);

            var recipes = _electricityRepository.GetRecipes();

            var result = _electricityRepository.Calculate(danish, elpriser, recipes);

            _baseRepository.BulkMerge(result);

            _logger.Debug("Calculate schedules ends");
        }

        public void UpdateConsumption(int deviceId, IReadOnlyCollection<long> list)
        {
            long[] array = list.ToArray();

            var cultureInfo = new CultureInfo("da-DK");

            var firstDate = DateTime.ParseExact(array[0].ToString(), "yyMMdd", cultureInfo);
            //    var secondDate = DateTime.ParseExact(array[24].ToString(), "yyMMdd", cultureInfo);

            var c = _electricityRepository.GetConsumptionByIdAndDate(firstDate, deviceId);

            if (c != null)
            {
                c.H00 = Math.Max(c.H00, array[1]);
                c.H01 = Math.Max(c.H01, array[2]);
                c.H02 = Math.Max(c.H02, array[3]);
                c.H03 = Math.Max(c.H03, array[4]);
                c.H04 = Math.Max(c.H04, array[5]);
                c.H05 = Math.Max(c.H05, array[6]);
                c.H06 = Math.Max(c.H06, array[7]);
                c.H07 = Math.Max(c.H07, array[8]);
                c.H08 = Math.Max(c.H08, array[9]);
                c.H09 = Math.Max(c.H09, array[10]);
                c.H10 = Math.Max(c.H10, array[11]);
                c.H11 = Math.Max(c.H11, array[12]);
                c.H12 = Math.Max(c.H12, array[13]);
                c.H13 = Math.Max(c.H13, array[14]);
                c.H14 = Math.Max(c.H14, array[15]);
                c.H15 = Math.Max(c.H15, array[16]);
                c.H16 = Math.Max(c.H16, array[17]);
                c.H17 = Math.Max(c.H17, array[18]);
                c.H18 = Math.Max(c.H18, array[19]);
                c.H19 = Math.Max(c.H19, array[20]);
                c.H20 = Math.Max(c.H20, array[21]);
                c.H21 = Math.Max(c.H21, array[22]);
                c.H22 = Math.Max(c.H22, array[23]);
                c.H23 = Math.Max(c.H23, array[24]);
                _electricityRepository.UpdateConsumption(c);
            }
            else
            {
                c = new Consumption()
                {
                    Date = firstDate,
                    DeviceId = deviceId,
                    LastUpdatedUtc = DateTime.UtcNow,
                    H00 = array[1],
                    H01 = array[2],
                    H02 = array[3],
                    H03 = array[4],
                    H04 = array[5],
                    H05 = array[6],
                    H06 = array[7],
                    H07 = array[8],
                    H08 = array[9],
                    H09 = array[10],
                    H10 = array[11],
                    H11 = array[12],
                    H12 = array[13],
                    H13 = array[14],
                    H14 = array[15],
                    H15 = array[16],
                    H16 = array[17],
                    H17 = array[18],
                    H18 = array[19],
                    H19 = array[20],
                    H20 = array[21],
                    H21 = array[22],
                    H22 = array[23],
                    H23 = array[24],
                };
                _electricityRepository.InsertConsumption(c);

            }

        }

        public IReadOnlyCollection<ScheduleDto> GetSchedulesForPeriod(int deviceId, DateTime fromDateUtc, DateTime toDateUtc)
        {
            return _electricityRepository.GetSchedulesForPeriod(deviceId, fromDateUtc, toDateUtc); 
        }
    }
}
