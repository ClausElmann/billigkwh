using System;
using System.Collections.Generic;

namespace BilligKwhWebApp.Services.Invoicing.Dto
{
    public class FileInvoiceDTO
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public DateTime dateFromUTC { get; set; }
        public DateTime dateToUTC { get; set; }
        public ICollection<InvoiceEntryDTO> InvoiceEntries { get; set; } = new List<InvoiceEntryDTO>();
    }
}
