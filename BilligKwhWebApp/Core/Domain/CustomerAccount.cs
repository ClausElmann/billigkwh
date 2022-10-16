using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class CustomerAccount : BaseEntity
    {
        public decimal SubscriptionPrice { get; set; }
        public decimal SubscriptionPriceIncrease { get; set; }
        public DateTime? SubscriptionPriceIncreaseUTC { get; set; }
        public decimal PricePerSms { get; set; }
        public decimal PricePerVoice { get; set; }
        public decimal PricePerEmail { get; set; }
        public decimal PricePerEboks { get; set; }
        public bool DetailedInvoice { get; set; }
        public string CustomerAccountType { get; set; }
        // Relations
        public int CustomerId { get; set; }

        // Ctor
        public CustomerAccount()
        {

        }
        public CustomerAccount(decimal price, decimal priceIncrease, decimal smsPrice, decimal voicePrice, decimal emailPrice, decimal eboksPrice) {
            SetSubscriptionPrice(price);
            SetSubscriptionPriceIncrease(priceIncrease);
            SetSmsPrice(smsPrice);
            SetVoiceSmsPrice(voicePrice);
            SetEmailPrice(emailPrice);
            SetEboksPrice(eboksPrice);
        }

        // ToDo: Validate
        public CustomerAccount SetSubscriptionPrice(decimal price)
        {
            // Validiate..
            SubscriptionPrice = price;
            return this;
        }
        public CustomerAccount SetSubscriptionPriceIncrease(decimal price)
        {
            SubscriptionPriceIncrease = price;
            return this;
        }
        public CustomerAccount SetPriceIncreaseDate(DateTime? date)
        {
            // Validate..
            SubscriptionPriceIncreaseUTC = date;
            return this;
        }
        public CustomerAccount SetSmsPrice(decimal price)
        {
            PricePerSms = price;
            return this;
        }
        public CustomerAccount SetVoiceSmsPrice(decimal price)
        {
            PricePerVoice = price;
            return this;
        }
        public CustomerAccount SetEmailPrice(decimal price)
        {
            PricePerEmail = price;
            return this;
        }
        public CustomerAccount SetEboksPrice(decimal price)
        {
            PricePerEboks = price;
            return this;
        }

        public CustomerAccount SetDetailInvoice(bool newValue)
        {
            DetailedInvoice = newValue;
            return this;
        }
    }
}
