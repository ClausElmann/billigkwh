using System;

namespace BilligKwhWebApp.Services.Invoicing.Economic.Customers
{
    public class EconomicPaymentTerms
    {
        public EconomicPaymentTerms()
        {
        }

        public EconomicPaymentTerms(int paymentId)
        {
            PaymentTermsNumber = paymentId;
            Self = new Uri($"https://restapi.e-conomic.com/payment-terms/{paymentId}");
        }

        public int PaymentTermsNumber { get; set; }
        public Uri Self { get; set; }
    }
}