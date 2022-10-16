//using BilligKwhWebApp.Services.Invoicing.Economic.Infrastructure;
//using BilligKwhWebApp.Services.Invoicing.Models;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using BilligKwhWebApp.Services.Customers;
//using BilligKwhWebApp.Core.Domain;
//using BilligKwhWebApp.Services.Interfaces;

//namespace BilligKwhWebApp.Services.Invoicing
//{
//    public class EconomicBackgroundService : BackgroundService
//    {
//        private readonly IServiceProvider _serviceProvider;
//        private readonly EconomicReportProcessingChannel _economicReportProcessingChannel;

//        private readonly IEnumerable<Kunde> Customers = new List<Kunde>();

//        // Ctor
//        public EconomicBackgroundService(
//            IServiceProvider serviceProvider,
//            IEconomicHttpClient economicHttpClient,
//            EconomicReportProcessingChannel economicReportProcessingChannel)
//        {
//            // Deps
//            _serviceProvider = serviceProvider;
//            _economicReportProcessingChannel = economicReportProcessingChannel;

//            // Create a local serviceProvider for scoped services
//            using var scope = _serviceProvider.CreateScope();
//            // Local Dependencies
//            var _customerService = scope.ServiceProvider.GetRequiredService<ICustomerService>();

//            Customers = _customerService.GetAll();
//        }

//        // Api
//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            // Force to run async
//            await Task.Yield();

//            // Create a local serviceProvider for scoped services
//            using var scope = _serviceProvider.CreateScope();
//            // Local Dependencies
//            var _invoicingService = scope.ServiceProvider.GetRequiredService<IInvoicingService>();
//            var _economicHttpClient = scope.ServiceProvider.GetRequiredService<IEconomicHttpClient>();

//            while (!stoppingToken.IsCancellationRequested)
//            {
//                try
//                {
//                    // Create async Stream to handle the background thread.
//                    await foreach (var economicReport in _economicReportProcessingChannel.ReadAllAsync(stoppingToken))
//                    {
//                        try
//                        {
//                            // Manage Account
//                            if (economicReport.InvoiceDrafts.Any())
//                            {
//                                // Extract the Invoice Draft from the Economic Report
//                                var invoiceDraftTemplate = economicReport.InvoiceDrafts.FirstOrDefault();

//                                // Update The InvoiceLines..
//                                var economicInvoiceDraft = await _economicHttpClient.GetInvoiceDraft(invoiceDraftTemplate.TemporaryDraftId).ConfigureAwait(false);
//                                economicInvoiceDraft.UpdateNotes(economicReport);
//                                //economicInvoiceDraft.UpdateDanishInvoiceLines(economicReport, customerAccount);

//                                // Transfer to Economic.com and save the response
//                                var result = await _economicHttpClient.UpdateInvoiceDraft(economicInvoiceDraft, stoppingToken).ConfigureAwait(false);

//                                // Unwrap and Save the economic Response
//                                InvoiceEconomicTransferResult transferResult;
//                                if (result.IsSuccess) transferResult = _invoicingService.BuildTransferResultModel(economicReport, result.Value);
//                                else transferResult = _invoicingService.BuildTransferResultModel(economicReport, 400, result.Message);

//                                _invoicingService.CreateInvoiceTransferResult(transferResult);
//                            }
//                        }
//                        catch (Exception e)
//                        {
//                            // Initialize the Logger and Write Err Message.
//                            var _logger = scope.ServiceProvider.GetRequiredService<ISystemLogger>();
//                            _logger.Fatal($"Transfer to Economic.com stopped in progress - {e.Message}", e);
//                        }
//                    }
//                }
//                catch (OperationCanceledException oce)
//                {
//                    var _logger = scope.ServiceProvider.GetRequiredService<ISystemLogger>();
//                    _logger.Information($"Economic.com Background Service was stopped - {oce.Message}", oce.StackTrace);
//                }
//                catch (Exception e)
//                {
//                    // Initialize the Logger and Write Err Message.
//                    var _logger = scope.ServiceProvider.GetRequiredService<ISystemLogger>();
//                    _logger.Fatal($"Economic.com Background Service failed - {e.Message}", e);
//                }
//            }
//            await Task.CompletedTask.ConfigureAwait(false);
//        }


//        private bool IsCustomerEconomicIdUnique(int economicId)
//        {
//            var test = Customers.Where(customer => customer.EconomicId == economicId);

//            if (test.Count() > 1) return false;

//            return true;
//        }
//    }
//}