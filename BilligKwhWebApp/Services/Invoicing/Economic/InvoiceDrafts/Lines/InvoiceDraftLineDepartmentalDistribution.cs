using System.Text.Json.Serialization;

namespace Balarm.Services.Invoicing.Economic.InvoiceDrafts.Lines
{
    public class InvoiceDraftLineDepartmentalDistribution
    {
        [JsonPropertyName("departmentalDistributionNumber")]
        public int? DepartmentalDistributionId { get; set; }

        public string DistributionType { get; set; }
    }
}
