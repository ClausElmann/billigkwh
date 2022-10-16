using BilligKwhWebApp.Core.Domain.Invoicing;
using System.Collections.Generic;

namespace BilligKwhWebApp.Services.Invoicing.Models
{
    public class AllInvoicingWrapper
    {
        public int PseudoId { get; set; }
        public List<InvoiceFile> InvoiceFiles { get; set; }

        public AllInvoicingWrapper()
        {
            InvoiceFiles = new List<InvoiceFile>();
        }
    }
}
