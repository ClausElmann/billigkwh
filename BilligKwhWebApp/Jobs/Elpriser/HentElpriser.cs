//using BilligKwhWebApp.Services;
//using Microsoft.Extensions.Logging;
//using System.Threading.Tasks;
//using System.Threading;
//using System;
//using System.Net.Http;
//using System.Text.Json;
//using BilligKwhWebApp.Core.Interfaces;
//using BilligKwhWebApp.Core.Domain;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.Extensions.DependencyInjection;
//using BilligKwhWebApp.Jobs.ElectricityPrices;
//using BilligKwhWebApp.Services.Interfaces;
//using BilligKwhWebApp.Services.Electricity.Repository;

//namespace BilligKwhWebApp.Jobs
//{
//    public class HentElectricityPrices : CronJobService
//    {
//        private readonly IServiceProvider _serviceProvider;

//        public HentElectricityPrices(IScheduleConfig<HentElectricityPrices> config, IServiceProvider serviceProvider)
//            : base(config.CronExpression, config.TimeZoneInfo)
//        {
//            _serviceProvider = serviceProvider;
//        }

//        public override Task StartAsync(CancellationToken cancellationToken)
//        {
//            return base.StartAsync(cancellationToken);
//        }

//        public override async Task DoWork(CancellationToken cancellationToken)
//        {
//            try
//            {
//                using (var scope = _serviceProvider.CreateScope())
//                {
//                    var _systemLogger = scope.ServiceProvider.GetRequiredService<ISystemLogger>();
                   
//                    _systemLogger.Debug("Hent elpriser starts.");

//                    var _baseRepository = scope.ServiceProvider.GetRequiredService<IBaseRepository>();
//                    HttpClient client = new();
//                    //HttpResponseMessage response = await client.GetAsync($"https://api.energidataservice.dk/dataset/Elspotprices?offset=0&start=2022-01-01T00:00&filter=%7B%22PriceArea%22:%22dk1,dk2%22%7D&sort=HourUTC%20ASC&timezone=dk", cancellationToken);
//                     HttpResponseMessage response = await client.GetAsync($"https://api.energidataservice.dk/dataset/Elspotprices?offset=0&start={DateTime.UtcNow:yyyy-MM-dd}T00:00&filter=%7B%22PriceArea%22:%22dk1,dk2%22%7D&sort=HourUTC%20ASC&timezone=dk", cancellationToken);
//                    response.EnsureSuccessStatusCode();
//                    string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

//                    var welcome = JsonSerializer.Deserialize<Root>(responseBody);

//                    List<ElectricityPrice> ElectricityPrices = new();

//                    DateTime updated = DateTime.UtcNow;

//                    foreach (var record in welcome.records.Where(w => w.PriceArea == "DK1"))
//                    {
//                        var Dk2 = welcome.records.Where(w => w.HourDK == record.HourDK && w.PriceArea == "DK2").FirstOrDefault();
//                        ElectricityPrices.Add(new ElectricityPrice()
//                        {
//                            DatoUtc = record.HourUTC,
//                            DateDk = record.HourDK.Date,
//                            TimeDk = record.HourDK.Hour,
//                            Dk1 = (decimal)record.SpotPriceDKK / 1000,
//                            Dk2 = (decimal)Dk2.SpotPriceDKK / 1000,
//                            Updated = updated,
//                        });
//                    }
//                    _baseRepository.BulkMerge(ElectricityPrices);

//                    _systemLogger.Debug("Hent elpriser ends, Starting to calculate schedules.");

//                    DateTime danish = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Romance Standard Time");

//                    var _electricityRepository = scope.ServiceProvider.GetRequiredService<IElectricityRepository>();

//                    var elpriser = _electricityRepository.GetElectricityPriceForDate(danish.Date);

//                    var recipes = _electricityRepository.GetRecipes();

//                    var result = _electricityRepository.Calculate(danish, elpriser, recipes);

//                    _baseRepository.BulkMerge(result);

//                    _systemLogger.Debug("Calculate schedules ends");
//                }
//            }
//            catch (Exception)
//            {
//            }
//            return;
//        }

//        public override Task StopAsync(CancellationToken cancellationToken)
//        {
//            return base.StopAsync(cancellationToken);
//        }
//    }
//}
