using BilligKwhWebApp.Core.Domain.ValueObjects;
using BilligKwhWebApp.Core.Toolbox;
using System;

namespace BilligKwhWebApp.Services.Invoicing.Economic.InvoiceDrafts.Lines
{
    public class InvoiceDraftLineAccrual : ValueObject<InvoiceDraftLineAccrual>
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        // Ctor
        public InvoiceDraftLineAccrual()
        {

        }
        public InvoiceDraftLineAccrual(string startDate, string endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
        public InvoiceDraftLineAccrual(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate.ToString();
            EndDate = endDate.ToString();
        }

        // Api
        public InvoiceDraftLineAccrual ConvertFromUTC(int toCountryId)
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

            StartDate = DateTime.SpecifyKind(DateTime.Parse(StartDate), DateTimeKind.Utc).FromUtcToLocalDate(timeZone).ToString();
            EndDate = DateTime.SpecifyKind(DateTime.Parse(EndDate), DateTimeKind.Utc).FromUtcToLocalDate(timeZone).ToString();

            return this;
        }


        // Base ValueObject Implementation
        protected override bool EqualsCore(InvoiceDraftLineAccrual other)
        {
            return StartDate != null && EndDate != null
                ? StartDate.Equals(other.StartDate) && EndDate.Equals(other.EndDate)
                : false;
        }
        protected override int GetHashCodeCore()
        {
            return (StartDate + EndDate).GetHashCode();
        }
    }
}
