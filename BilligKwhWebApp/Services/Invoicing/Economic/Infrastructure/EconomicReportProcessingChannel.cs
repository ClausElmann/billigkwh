using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Services.Invoicing.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace BilligKwhWebApp.Services.Invoicing.Economic.Infrastructure
{
    public class EconomicReportProcessingChannel
    {
        // Props
        private const int _channelCapacity = 2500;
        private readonly Channel<EconomicReportDTO> _channel;

        // Ctor
        public EconomicReportProcessingChannel()
        {
            // Create a Bounded Channel
            var options = new BoundedChannelOptions(_channelCapacity)
            {
                SingleWriter = false,
                SingleReader = true
            };

            _channel = Channel.CreateBounded<EconomicReportDTO>(options);
        }

        // Commands
        public async Task<Result> AddEconomicReport(EconomicReportDTO report, CancellationToken ct = default) 
        {
            // Await the channel if Capacity is full
            while (await _channel.Writer.WaitToWriteAsync(ct).ConfigureAwait(false) && !ct.IsCancellationRequested)
            {
                // Write the Report to the channel
                if (_channel.Writer.TryWrite(report))
                {
                    return Result.Ok();
                }
            }
            return Result.Fail("An Error Happend in the EconomicReport Processing Channel.");
        }
        public async Task<Result> AddEconomicReport(IEnumerable<EconomicReportDTO> reports, CancellationToken ct = default)
        {
            foreach (var economicReport in reports)
            {
                // Await the channel if Capacity is full
                if (!await _channel.Writer.WaitToWriteAsync(ct).ConfigureAwait(false))
                {
                    return Result.Fail("An error happened writing to the EconomicReport Processing Channel.");
                }
                if (ct.IsCancellationRequested)
                {
                    return Result.Fail("Action cancelled while writing to the EconomicReport Processing Channel.");
                }
                if (!_channel.Writer.TryWrite(economicReport))
                {
                    return Result.Fail("An error happened writing to the EconomicReport Processing Channel.");
                }
            }
            return Result.Ok();
        }

        // Queries
        public IAsyncEnumerable<EconomicReportDTO> ReadAllAsync(CancellationToken ct = default) => _channel.Reader.ReadAllAsync(ct);
    }
}
