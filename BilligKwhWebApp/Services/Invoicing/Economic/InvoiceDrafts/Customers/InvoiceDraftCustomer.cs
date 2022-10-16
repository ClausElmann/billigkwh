using System.Text.Json.Serialization;

namespace BilligKwhWebApp.Services.Invoicing.Economic.InvoiceDrafts.Customers
{
    public class InvoiceDraftCustomer
    {
        [JsonPropertyName("customerNumber")]
        public int CustomerEconomicId { get; set; }
    }
}
