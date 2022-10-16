using BilligKwhWebApp.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using BilligKwhWebApp.Services.Interfaces;
using BilligKwhWebApp.Core.Toolbox;

namespace BilligKwhWebApp.Jobs
{
    public class MyCronJob3 : CronJobService
    {
        private readonly ILogger<MyCronJob3> _logger;
        private readonly IServiceProvider _serviceProvider;

        public MyCronJob3(IScheduleConfig<MyCronJob3> config, ILogger<MyCronJob3> logger, IServiceProvider serviceProvider)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 3 starts.");
            return base.StartAsync(cancellationToken);
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _baseRepository = scope.ServiceProvider.GetRequiredService<IBaseRepository>();
                    HttpClient client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync($"https://api.energidataservice.dk/dataset/Elspotprices?offset=0&start={DateTime.UtcNow:yyyy-MM-dd}T00:00&filter=%7B%22PriceArea%22:%22dk1,dk2%22%7D&sort=HourUTC%20ASC&timezone=dk");
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

                    var welcome = JsonSerializer.Deserialize<Root>(responseBody);

                    List<ElPris> Elpriser = new();

                    DateTime updated = DateTime.UtcNow;

                    foreach (var record in welcome.records.Where(w => w.PriceArea == "DK1"))
                    {
                        var Dk2 = welcome.records.Where(w => w.HourDK == record.HourDK && w.PriceArea == "DK2").FirstOrDefault();
                        Elpriser.Add(new ElPris()
                        {
                            DatoUtc = record.HourUTC,
                            TimeDk = record.HourDK.Hour,
                            Dk1 = (decimal)record.SpotPriceDKK / 1000,
                            Dk2 = (decimal)Dk2.SpotPriceDKK / 1000,
                            Updated = updated,
                        });
                    }
                    _baseRepository.BulkMerge(Elpriser);
                    return;
                }
            }
            catch (Exception)
            {
            }
            return;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 3 is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
