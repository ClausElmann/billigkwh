using System.Text.Json.Serialization;

namespace BilligKwhWebApp.Services.Invoicing.Economic.InvoiceDrafts.Lines
{
    public class InvoiceDraftLineUnit
    {
        [JsonPropertyName("unitNumber")]
        public int UnitId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Name { get; set; }

        public InvoiceDraftLineUnit()
        {

        }
        public InvoiceDraftLineUnit(int unitId)
        {
            UnitId = unitId;
        }
    }
}
