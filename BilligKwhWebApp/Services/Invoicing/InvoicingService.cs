using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Domain.Invoicing;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Services.Invoicing.Dto;
using BilligKwhWebApp.Services.Invoicing.Economic.Customers;
using BilligKwhWebApp.Services.Invoicing.Economic.InvoiceDrafts;
using BilligKwhWebApp.Services.Invoicing.Economic.InvoiceDrafts.Lines;
using BilligKwhWebApp.Services.Invoicing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BilligKwhWebApp.Services.Invoicing
{
    public interface IInvoicingService
    {
        // Invoices Old
        IEnumerable<FileInvoiceDTO> ListInvoiceFiles();
        void DeleteInvoiceFile(int fileId);

        // Domain
        InvoiceFile GetInvoiceFile(int fileId);
        IEnumerable<InvoiceFile> GetAllInvoicingFiles();

        IEnumerable<EconomicReportDTO> GroupEconomicReportsByEconomicId(IEnumerable<EconomicReportDTO> economicReports);

        // Invoice Transfer Results
        void CreateInvoiceTransferResult(InvoiceEconomicTransferResult transferResult);

        InvoiceEconomicTransferResult BuildTransferResultModel(EconomicReportDTO economicReport, HttpResponseMessage reponseMessage);
        InvoiceEconomicTransferResult BuildTransferResultModel(EconomicReportDTO economicReport, int responseCode, string reponseMessage);

        IEnumerable<InvoiceEconomicTransferResult> GetTransferResults(int economicId);
        IEnumerable<InvoiceEconomicTransferResult> GetTransferResults(int economicId, int statusCode);
        IEnumerable<InvoiceEconomicTransferResult> GetTransferResults(int economicId, int statusCode, InvoiceDraftLineAccrual accrual);
        IEnumerable<InvoiceEconomicTransferResult> GetTransferResults(DateTime accrualStartUTC, DateTime accrualEndUTC);
        Task<byte[]> GetSentOrderPdf(int orderNumber);
        Task<byte[]> GetDraftInvoicePdf(ElTavle eltavle);
        Task<byte[]> GetBookedInvoicePdf(ElTavle eltavle);
    }

    public class InvoicingService : IInvoicingService
    {
        private readonly IBaseRepository _baseRepository;
        private readonly IEconomicHttpClient _economicClient;

        public InvoicingService(IBaseRepository baseRepository, IEconomicHttpClient economicClient)
        {
            _baseRepository = baseRepository;
            _economicClient = economicClient;
        }

        // Api: Invoices_Old (Importing of csv_file from the old system)
        public IEnumerable<InvoiceFile> GetAllInvoicingFiles()
        {

            var sql = @"SELECT 
                            1 AS PseudoId,
                            invoiceFile.Id,
                            invoiceFile.FileName,
                            invoiceFile.DateFromUTC,
                            invoiceFile.DateToUTC,
                            invoiceEntry.Id,
                            invoiceEntry.CustomerName,
                            invoiceEntry.EconomicId,
                            invoiceEntry.SmsCount,
                            invoiceEntry.InvoiceFileId
                        FROM dbo.InvoiceFiles_Old invoiceFile
	                        INNER JOIN dbo.InvoiceEntries_Old invoiceEntry ON invoiceEntry.InvoiceFileId = invoiceFile.Id";

            // Lookup
            var invoiceWrapperLookup = new Dictionary<int, AllInvoicingWrapper>();
            var invoiceFileLookup = new Dictionary<int, InvoiceFile>();
            var invoiceEntryLookup = new Dictionary<int, InvoiceEntry>();

            // MultiMap
            var result = _baseRepository.Query<AllInvoicingWrapper, InvoiceFile, InvoiceFileInfo, InvoiceEntry>(sql, (wrapper, invoiceFile, fileInfo, invoiceEntry) =>
            {
                // Lookup Wrapper
                if (!invoiceWrapperLookup.TryGetValue(wrapper.PseudoId, out var currentWrapper))
                {
                    invoiceWrapperLookup.Add(wrapper.PseudoId, currentWrapper = wrapper);
                }
                // LookUp Files
                if (!invoiceFileLookup.TryGetValue(invoiceFile.Id, out var currentInvoiceFile))
                {
                    invoiceFileLookup.Add(invoiceFile.Id, currentInvoiceFile = invoiceFile);
                    currentInvoiceFile.FileInfo = fileInfo;

                    currentWrapper.InvoiceFiles.Add(currentInvoiceFile);
                }
                // Add File Entries
                currentInvoiceFile.InvoiceEntries.Add(invoiceEntry);

                return wrapper;
            }, "Id, FileName, Id");

            if (!result.Any()) return new List<InvoiceFile>();

            return result.FirstOrDefault().InvoiceFiles;
        }
        public InvoiceFile GetInvoiceFile(int fileId)
        {
            var param = new { FileId = fileId };
            var sql = @"SELECT 
                            invoiceFile.Id,
                            invoiceFile.FileName,
                            invoiceFile.DateFromUTC,
                            invoiceFile.DateToUTC,
                            invoiceEntry.Id,
                            invoiceEntry.CustomerName,
                            invoiceEntry.EconomicId,
                            invoiceEntry.SmsCount,
                            invoiceEntry.InvoiceFileId
                        FROM dbo.InvoiceFiles_Old invoiceFile
	                        INNER JOIN dbo.InvoiceEntries_Old invoiceEntry ON invoiceEntry.InvoiceFileId = invoiceFile.Id
                        WHERE invoiceFile.Id = @FileId";

            // Lookup
            var invoiceFileLookup = new Dictionary<int, InvoiceFile>();
            var invoiceEntryLookup = new Dictionary<int, InvoiceEntry>();

            // MultiMap
            return _baseRepository.Query<InvoiceFile, InvoiceFileInfo, InvoiceEntry>(sql, (invoiceFile, fileInfo, invoiceEntry) =>
            {
                // LookUp Files
                if (!invoiceFileLookup.TryGetValue(invoiceFile.Id, out var currentInvoiceFile))
                {
                    invoiceFileLookup.Add(invoiceFile.Id, currentInvoiceFile = invoiceFile);
                    currentInvoiceFile.FileInfo = fileInfo;
                }
                // Add File Entries
                currentInvoiceFile.InvoiceEntries.Add(invoiceEntry);

                return invoiceFile;
            }, "Id, FileName, Id", param).FirstOrDefault();
        }

        public IEnumerable<EconomicReportDTO> GroupEconomicReportsByEconomicId(IEnumerable<EconomicReportDTO> economicReports)
        {
            return economicReports
                        .Where(report => report.InvoiceDrafts.Any())
                        .GroupBy(report => new
                        {
                            report.EconomicId,
                            report.CountryId,
                            report.Accrual,
                            report.InvoiceDrafts.First().TemporaryDraftId,
                        })
                        .Select(group => new EconomicReportDTO
                        {
                            EconomicId = group.Key.EconomicId,
                            CountryId = group.Key.CountryId,
                            Accrual = group.Key.Accrual,
                            SmsCount = group.Sum(report => report.SmsCount),
                            EmailCount = group.Sum(report => report.EmailCount),
                            SmsVoiceCount = group.Sum(report => report.SmsVoiceCount),
                            InvoiceDrafts = new List<InvoiceDraft>() { new InvoiceDraft(group.Key.TemporaryDraftId) }
                        });
        }

        public IEnumerable<FileInvoiceDTO> ListInvoiceFiles()
        {
            var sql = @"SELECT * 
                        FROM dbo.InvoiceFiles_Old invoiceFile";

            return _baseRepository.Query<FileInvoiceDTO>(sql);
        }

        public void DeleteInvoiceFile(int fileId)
        {
            var param = new { FileId = fileId };
            var file = @"DELETE 
                        FROM [dbo].[InvoiceFiles_Old]
                        WHERE Id = @FileId";
            var fileEntries = @"DELETE 
                                FROM [dbo].[InvoiceEntries_Old]
                                WHERE InvoiceFileId = @FileId";


            _baseRepository.Execute(fileEntries, param);
            _baseRepository.Execute(file, param);
        }

        // InvoiceTransferResult CRUD
        public IEnumerable<InvoiceEconomicTransferResult> GetTransferResults(DateTime accrualStart, DateTime accrualEnd)
        {
            var param = new { AccrualStart = accrualStart, AccrualEnd = accrualEnd };
            var sql = @"SELECT * 
                        FROM dbo.InvoiceEconomicTransferResult invoiceResult
                        WHERE MONTH(invoiceResult.AccrualEnd) = MONTH(@AccrualEnd)
                            AND YEAR(invoiceResult.AccrualEnd) = YEAR(@AccrualEnd)";

            return _baseRepository.Query<InvoiceEconomicTransferResult>(sql, param);
        }

        public IEnumerable<InvoiceEconomicTransferResult> GetTransferResults(int economicId)
        {
            var param = new { EconomicId = economicId };
            var sql = @"SELECT * 
                        FROM dbo.InvoiceEconomicTransferResult invoiceResult
                        WHERE invoiceResult.EconomicId = @EconomicId";

            return _baseRepository.Query<InvoiceEconomicTransferResult>(sql, param);
        }
        public IEnumerable<InvoiceEconomicTransferResult> GetTransferResults(int economicId, int statusCode)
        {
            var param = new { EconomicId = economicId, ResponseCode = statusCode };
            var sql = @"SELECT *
                        FROM dbo.InvoiceEconomicTransferResult invoiceResult
                        WHERE invoiceResult.EconomicId = @EconomicId
                            AND invoiceResult.ResponseCode = @ResponseCode";

            return _baseRepository.Query<InvoiceEconomicTransferResult>(sql, param);
        }
        public IEnumerable<InvoiceEconomicTransferResult> GetTransferResults(int economicId, int statusCode, InvoiceDraftLineAccrual accrual)
        {
            var param = new
            {
                EconomicId = economicId,
                ResponseCode = statusCode,
                AccrualStartMonth = DateTime.Parse(accrual.StartDate).Month,
                AccrualStartYear = DateTime.Parse(accrual.StartDate).Year,
                AccrualEndMonth = DateTime.Parse(accrual.EndDate).Month,
                AccrualEndYear = DateTime.Parse(accrual.EndDate).Year
            };

            var sql = @"SELECT *
                        FROM dbo.InvoiceEconomicTransferResult invoiceResult
                        WHERE invoiceResult.EconomicId = @EconomicId
                            AND invoiceResult.ResponseCode = @ResponseCode
                            AND MONTH(invoiceResult.AccrualStart) = @AccrualStartMonth
                            AND YEAR(invoiceResult.AccrualStart) = @AccrualStartYear
                            AND MONTH(invoiceResult.AccrualEnd) = @AccrualEndMonth
                            AND YEAR(invoiceResult.AccrualEnd) = @AccrualEndYear";

            return _baseRepository.Query<InvoiceEconomicTransferResult>(sql, param);
        }

        public void CreateInvoiceTransferResult(InvoiceEconomicTransferResult transferResult)
        {
            _baseRepository.Insert(transferResult);
        }

        public InvoiceEconomicTransferResult BuildTransferResultModel(EconomicReportDTO economicReport, HttpResponseMessage reponseMessage)
        {
            return new InvoiceEconomicTransferResult
            {
                EconomicId = economicReport.EconomicId,
                ResponseCode = (int)reponseMessage.StatusCode,
                ResponseMessage = reponseMessage.ReasonPhrase,
                AccrualStart = DateTime.Parse(economicReport.Accrual.StartDate),
                AccrualEnd = DateTime.Parse(economicReport.Accrual.EndDate),
                CreatedDateUTC = DateTime.UtcNow
            };
        }
        public InvoiceEconomicTransferResult BuildTransferResultModel(EconomicReportDTO economicReport, int responseCode, string reponseMessage)
        {
            return new InvoiceEconomicTransferResult
            {
                EconomicId = economicReport.EconomicId,
                ResponseCode = responseCode,
                ResponseMessage = reponseMessage,
                AccrualStart = DateTime.Parse(economicReport.Accrual.StartDate),
                AccrualEnd = DateTime.Parse(economicReport.Accrual.EndDate),
                CreatedDateUTC = DateTime.UtcNow
            };
        }

        public async Task<byte[]> GetSentOrderPdf(int economicOrderNumber)
        {
            return await _economicClient.GetOrderPdf(economicOrderNumber).ConfigureAwait(false);
        }

        public async Task<byte[]> GetDraftInvoicePdf(ElTavle eltavle)
        {
                return await _economicClient.GetDraftInvoicePdf((int)eltavle.EconomicDraftInvoiceNumber).ConfigureAwait(false);
        }

        public async Task<byte[]> GetBookedInvoicePdf(ElTavle eltavle)
        {
            return await _economicClient.GetBookedInvoicePdf((int)eltavle.EconomicBookedInvoiceNumber).ConfigureAwait(false);
        }
    }
}
