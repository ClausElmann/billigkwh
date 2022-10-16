using System;

namespace BilligKwhWebApp.Services.Invoicing.Economic.Customers
{
    public class EconomicUnit
    {
        public EconomicUnit() { }

        public EconomicUnit(int unitNumber)
        {
            UnitNumber = unitNumber;
            Self = new Uri($"https://restapi.e-conomic.com/units/{unitNumber}");
        }

        public int UnitNumber { get; set; }
        public Uri Self { get; set; }

        public static readonly EconomicUnit StkEconomicUnit = new EconomicUnit(1);
    }
}