using BilligKwhWebApp.Core.Domain.ValueObjects;
using System;

namespace BilligKwhWebApp.Core.Domain.Invoicing
{
    public class InvoiceFileInfo : ValueObject<InvoiceFileInfo>
    {
        // Props
        public string FileName { get; set; }
        public DateTime DateFromUTC { get; set; }
        public DateTime DateToUTC { get; set; }

        // Ctor
        public InvoiceFileInfo()
        {

        }
        public InvoiceFileInfo(string fileName, DateTime dateFromUTC, DateTime dateToUTC)
        {
            FileName = fileName;
            DateFromUTC = dateFromUTC;
            DateToUTC = dateToUTC;
        }


        protected override bool EqualsCore(InvoiceFileInfo other)
        {
            throw new NotImplementedException();
        }
        protected override int GetHashCodeCore()
        {
            throw new NotImplementedException();
        }
    }
}
