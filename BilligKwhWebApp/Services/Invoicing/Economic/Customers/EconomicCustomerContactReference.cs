using System;

namespace BilligKwhWebApp.Services.Invoicing.Economic.Customers
{
    public class EconomicCustomerContactReference
    {
        public EconomicCustomerContactReference()
        {
        }

        public EconomicCustomerContactReference(int customerNumber, int customerContactNumber)
        {
            CustomerContactNumber = customerContactNumber;
            Self = new Uri($"https://restapi.e-conomic.com/customers/{customerNumber}/contacts/{customerContactNumber}");
        }

        public int CustomerContactNumber { get; set; }
        public Uri Self { get; set; }
    }
}