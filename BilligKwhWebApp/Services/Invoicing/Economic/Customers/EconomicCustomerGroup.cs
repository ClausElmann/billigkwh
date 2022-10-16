using System;

namespace BilligKwhWebApp.Services.Invoicing.Economic.Customers
{
    public class EconomicCustomerGroup
    {
        public int CustomerGroupNumber { get; set; }
        public string Name { get; set; }
        public Uri Self { get; set; }
    }
}