using System;

namespace BilligKwhWebApp.Services.Invoicing.Economic.Customers
{
    public class EconomicVatZone
    {
        public int VatZoneNumber { get; set; }
        public Uri Self { get; set; }

        public readonly static EconomicVatZone DomesticVatZone = new EconomicVatZone { VatZoneNumber = 1, Self = new Uri("https://restapi.e-conomic.com/vat-zones/1") };
    }
}