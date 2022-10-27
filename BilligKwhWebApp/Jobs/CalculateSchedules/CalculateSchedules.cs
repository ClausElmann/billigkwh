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
//using BilligKwhWebApp.Services.Electricity.Repository;
//using System.Collections;
//using BilligKwhWebApp.Core;

//namespace BilligKwhWebApp.Jobs.ElectricityPrices
//{
//    public class CalculateSchedules : CronJobService
//    {
//        private readonly ILogger<CalculateSchedules> _logger;
//        private readonly IServiceProvider _serviceProvider;

//        public CalculateSchedules(IScheduleConfig<CalculateSchedules> config, ILogger<CalculateSchedules> logger, IServiceProvider serviceProvider)
//            : base(config.CronExpression, config.TimeZoneInfo)
//        {
//            _logger = logger;
//            _serviceProvider = serviceProvider;
//        }

//        public override Task StartAsync(CancellationToken cancellationToken)
//        {
//            _logger.LogInformation("CalculateSchedules starts.");
//            return base.StartAsync(cancellationToken);
//        }

//        public override async Task DoWork(CancellationToken cancellationToken)
//        {
//            try
//            {
//                using (var scope = _serviceProvider.CreateScope())
//                {
//                    DateTime timeUtc = DateTime.UtcNow;
//                    TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
//                    DateTime danish = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(timeUtc, "Romance Standard Time");

//                    var _baseRepository = scope.ServiceProvider.GetRequiredService<IBaseRepository>();
//                    var _electricityRepository = scope.ServiceProvider.GetRequiredService<IElectricityRepository>();

//                    var elpriser = _electricityRepository.GetElectricityPriceForDate(danish.Date);

//                    var recipes = _electricityRepository.GetRecipes();

//                    var result = _electricityRepository.Calculate(danish, elpriser, recipes);

//                    _baseRepository.BulkMerge(result);
//                }
//            }
//            catch (Exception)
//            {
//            }
//            return;
//        }


//        public override Task StopAsync(CancellationToken cancellationToken)
//        {
//            _logger.LogInformation("CalculateSchedules is stopping.");
//            return base.StopAsync(cancellationToken);
//        }
//    }
//}
