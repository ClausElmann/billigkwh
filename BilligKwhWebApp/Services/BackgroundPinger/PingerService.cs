//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using System.Net.NetworkInformation;
//using System.Threading.Tasks;
//using System.Threading;
//using System;
//using BilligKwhWebApp.Services.Interfaces;
//using Microsoft.Extensions.DependencyInjection;

//namespace BilligKwhWebApp.Services.BackgroundPinger
//{
//    public class PingerService : BackgroundService
//    {
//        private readonly Ping _pinger;
//        private readonly IPingSettings _pingSettings;
//        private readonly IServiceProvider _serviceProvider;


//        public PingerService(IPingSettings pingSettings, IServiceProvider serviceProvider)
//        {
//            _pingSettings = pingSettings;
//            _pinger = new Ping();
//            _serviceProvider = serviceProvider;
//        }

//        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
//        {

//            using (var scope = _serviceProvider.CreateScope())
//            {
//                var _systemLogger = scope.ServiceProvider.GetRequiredService<ISystemLogger>();

//                while (!stoppingToken.IsCancellationRequested)
//                {
//                    await Task.Delay(_pingSettings.Frequency, stoppingToken);

//                    try
//                    {
//                        var pingTask = _pinger.SendPingAsync(_pingSettings.Target, (int)_pingSettings.Timeout.TotalMilliseconds);
//                        var cancelTask = Task.Delay(_pingSettings.Timeout, stoppingToken);

//                        //double await so exceptions from either task will bubble up
//                        await await Task.WhenAny(pingTask, cancelTask);

//                        if (pingTask.IsCompletedSuccessfully)
//                        {
//                            _systemLogger.Debug($"PingReply status={pingTask.Result.Status} roundTripTime={pingTask.Result.RoundtripTime}");
//                        }
//                        else
//                        {
//                            _systemLogger.Error("Ping didn't complete successfully");
//                        }

//                    }
//                    catch (Exception ex)
//                    {
//                        _systemLogger.Error("Ping didn't complete successfully", ex);
//                    }
//                }
//            }
//        }


//        public override void Dispose()
//        {
//            if (_pinger != null)
//            {
//                _pinger.Dispose();
//            }
//            base.Dispose();
//        }
//    }
//}
