using BilligKwhWebApp.Services.Invoicing.Economic.InvoiceDrafts;
using BilligKwhWebApp.Services.Invoicing.Economic.InvoiceDrafts.Lines;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace BilligKwhWebApp.Services.Invoicing.Dto
{
    public class EconomicReportDTO
    {
        public int EconomicId { get; set; }
        public int CountryId { get; set; }
        public string CustomerName { get; set; }
        public int SmsCount { get; set; }
        public int SmsVoiceCount { get; set; }
        public int EmailCount { get; set; }
        public InvoiceDraftLineAccrual Accrual { get; set; } // Create Base Accrual Value Object??

        [JsonPropertyName("invoiceEntries")]
        public IEnumerable<InvoiceDraft> InvoiceDrafts { get; set; }
    }
}
