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
        public virtual Customer Customer { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public CustomerUserMapping()
        {

        }
        public CustomerUserMapping(Customer customer, User user)
        {
            Customer = customer;
            User = user;
        }
    }
}
