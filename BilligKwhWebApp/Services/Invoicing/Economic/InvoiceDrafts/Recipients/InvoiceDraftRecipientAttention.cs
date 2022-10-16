using System.Text.Json.Serialization;

namespace BilligKwhWebApp.Services.Invoicing.Economic.InvoiceDrafts.Recipients
{
    public class InvoiceDraftRecipientAttention
    {
        [JsonPropertyName("customerContactNumber")]
        public int CustomerContactId { get; set; }
    }
}
