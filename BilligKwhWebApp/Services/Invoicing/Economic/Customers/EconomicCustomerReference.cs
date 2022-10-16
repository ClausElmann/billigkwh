using System;

namespace BilligKwhWebApp.Services.Invoicing.Economic.Customers
{
    public class EconomicCustomerReference
    {
        public EconomicCustomerReference() { }
        
        public EconomicCustomerReference(int customerNumber)
        {
            CustomerNumber = customerNumber;
            Self = new Uri($"https://restapi.e-conomic.com/customers/{customerNumber}");
        }

        public int CustomerNumber { get; set; }
        public Uri Self { get; set; }
    }
}