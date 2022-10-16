using BilligKwhWebApp.Core.Infrastructure;
using System.Collections.Generic;

namespace BilligKwhWebApp.Core.Domain.Invoicing
{
    public class InvoiceFile : AggregateRoot
    {
        // Props
        public InvoiceFileInfo FileInfo { get; set; }
        public ICollection<InvoiceEntry> InvoiceEntries { get; set; }

        // Ctor
        public InvoiceFile()
        {
            InvoiceEntries = new List<InvoiceEntry>();
        }
        public InvoiceFile(InvoiceFileInfo fileInfo)
        {
            FileInfo = fileInfo;
            InvoiceEntries = new List<InvoiceEntry>();
        }
        public InvoiceFile(InvoiceFileInfo fileInfo, ICollection<InvoiceEntry> invoiceEntries)
        {
            FileInfo = fileInfo;
            InvoiceEntries = invoiceEntries;
        }
    }
}
