using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Z.Dapper.Plus;

namespace BilligKwhWebApp.Services
{
    public class LocalDebugService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public LocalDebugService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;

            using var scope = _serviceProvider.CreateScope();

            //var _baseRepository = scope.ServiceProvider.GetRequiredService<IBaseRepository>();

            //var _eltavlerService = scope.ServiceProvider.GetRequiredService<IEltavleService>();

            //var komponenter = _eltavlerService.GetAllElTavleSektionKomponenterForPlacering(199);

            //List<ElTavleSektionElKomponent> komponentPlaceringer = new();

            //int placering = 0;

            //foreach (var f in komponenter)
            //{
            //    for (int i = 0; i < f.Antal; i++)
            //    {
            //        placering++;
            //        komponentPlaceringer.Add(new ElTavleSektionElKomponent()
            //        {
            //            ElTavleID = f.ElTavleID,
            //            ElTavleSektionID = f.ElTavleSektionID,
            //            KomponentID = f.KomponentID,
            //            KundeID = f.KundeID,
            //            Placering = placering,
            //            Navn = "",
            //            Line = 1
            //        });
            //    }
            //}

            //using (var connection = ConnectionFactory.GetOpenConnection())
            //using (var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            //{
            //    transaction.BulkInsert(komponentPlaceringer);
            //    transaction.Commit();
            //}

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
