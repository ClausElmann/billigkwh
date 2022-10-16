using System.Text.Json.Serialization;

namespace Balarm.Services.Invoicing.Economic.InvoiceDrafts.References
{
    public class InvoiceDraftReferenceSalesPerson
    {
        [JsonPropertyName("employeeNumber")]
        public int EmployeeId { get; set; }
    }
}
