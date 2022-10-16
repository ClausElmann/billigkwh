using System.Text.Json.Serialization;

namespace BilligKwhWebApp.Services.Invoicing.Economic.Customers
{
    public class EconomicCustomerContact
    {
        // Required for create
        public EconomicCustomerReference Customer { get; set; }
        public string Name { get; set; }

        // Optional for create
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Email { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Phone { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string MobilePhone { get; set; }
        // Required for update
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? CustomerContactNumber { get; set; }
    }
}
