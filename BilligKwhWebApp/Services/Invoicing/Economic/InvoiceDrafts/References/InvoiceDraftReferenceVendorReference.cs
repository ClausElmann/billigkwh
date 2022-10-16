using System.Text.Json.Serialization;

namespace BilligKwhWebApp.Services.Invoicing.Economic.InvoiceDrafts.References
{
    public class InvoiceDraftReferenceVendorReference
    {
        [JsonPropertyName("employeeNumber")]
        public int EmployeeId { get; set; }
    }
}
