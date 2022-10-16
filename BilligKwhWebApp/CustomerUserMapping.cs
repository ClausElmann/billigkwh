using BilligKwhWebApp.Core.Domain;
using System;

namespace BilligKwhWebApp
{
    // relation * to *
    public class CustomerUserMapping : BaseEntity
    {
        // Log
        public DateTime DateCreatedUtc { get; set; }
        public DateTime? DateLastUpdatedUtc { get; set; }

        // Foreign Keys
        public int CustomerId { get; set; }
        public virtual Kunde Customer { get; set; }

        public int UserId { get; set; }
        public virtual Bruger User { get; set; }

        public CustomerUserMapping()
        {

        }
        public CustomerUserMapping(Kunde customer, Bruger user)
        {
            Customer = customer;
            User = user;
        }
    }
}
