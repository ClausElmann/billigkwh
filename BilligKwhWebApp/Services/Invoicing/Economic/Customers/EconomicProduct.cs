using System;

namespace BilligKwhWebApp.Services.Invoicing.Economic.Customers
{
    public class EconomicProduct
    {
        public EconomicProduct() { }
        public EconomicProduct(string productNumber)
        {
            ProductNumber = productNumber;
            Self = new Uri($"https://restapi.e-conomic.com/products/{productNumber}");
        }

        public string ProductNumber { get; set; }
        public string Name { get; set; }
        public Uri Self { get; set; }

        public readonly static string Fordelingstavle = "1";
        public readonly static string Gruppetavle = "2";
        public readonly static string Løsdelsalg = "9" +
            "";

        public static string GetSetupProductNumber(int vare)
        {
            return vare switch
            {
                1 => Fordelingstavle,
                2 => Gruppetavle,
                9 => Løsdelsalg,
                _ => Gruppetavle
            };
        }
    }
}