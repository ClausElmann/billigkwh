using BilligKwhWebApp.Core.Domain.Invoicing;
using BilligKwhWebApp.Services.Customers;
using BilligKwhWebApp.Services.Invoicing.Dto;
using System.Linq;

namespace BilligKwhWebApp.Services.Invoicing
{
    public interface IInvoicingFactory 
    {
        FileInvoiceDTO ConvertToDTO(InvoiceFile invoiceFile);
    }

    public class InvoicingFactory : IInvoicingFactory
    {
        private readonly ICustomerService _customerService;

        public InvoicingFactory(ICustomerService customerService)
        {
            _customerService = customerService;}

        public FileInvoiceDTO ConvertToDTO(InvoiceFile invoiceFile)
        {
            var customers = _customerService.GetAll(true);
          
            return new FileInvoiceDTO
            {
                Id = invoiceFile.Id,
                FileName = invoiceFile.FileInfo.FileName,
                dateFromUTC = invoiceFile.FileInfo.DateFromUTC,
                dateToUTC = invoiceFile.FileInfo.DateToUTC,
                InvoiceEntries = invoiceFile.InvoiceEntries.Select(invoiceEntry => new InvoiceEntryDTO
                {
                    CustomerName = invoiceEntry.CustomerName,
                    EconomicId = invoiceEntry.EconomicId,
                    CountryId = customers.FirstOrDefault(customer => customer.EconomicId == invoiceEntry.EconomicId)?.LandID,
                    SmsCount = invoiceEntry.SmsCount
                }).ToList()
            };
        }
    }
}
