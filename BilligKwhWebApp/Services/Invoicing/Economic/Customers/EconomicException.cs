using System;

namespace BilligKwhWebApp.Services.Invoicing.Economic.Customers
{
    public class EconomicException : Exception
    {
        public EconomicException(string message) : base(message)
        {
        }
    }
}
