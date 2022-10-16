using System.Text.Json.Serialization;

namespace BilligKwhWebApp.Services.Invoicing.Economic.Customers
{
    public class EconomicOrderReferences
    {
        public EconomicCustomerContactReference CustomerContact { get; set; }






        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Other { get; set; }
    }
}