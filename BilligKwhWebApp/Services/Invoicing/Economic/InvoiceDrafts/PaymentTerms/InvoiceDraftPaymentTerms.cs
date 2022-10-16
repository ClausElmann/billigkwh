using System.Text.Json.Serialization;

namespace BilligKwhWebApp.Services.Invoicing.Economic.InvoiceDrafts.PaymentTerms
{
    public class InvoiceDraftPaymentTerms
    {
        [JsonPropertyName("paymentTermsNumber")]
        public int PaymentTermsId { get; set; }

        public string Name { get; set; }

        public int DaysOfCredit { get; set; }

        public string PaymentTermsType { get; set; }
    }
}
