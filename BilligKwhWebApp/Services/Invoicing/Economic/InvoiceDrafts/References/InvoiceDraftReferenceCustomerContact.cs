using System.Text.Json.Serialization;

namespace BilligKwhWebApp.Services.Invoicing.Economic.InvoiceDrafts.References
{
    public class InvoiceDraftReferenceCustomerContact
    {
        [JsonPropertyName("customerContactNumber")]
        public int CustomerContactId { get; set; }
    }
}
