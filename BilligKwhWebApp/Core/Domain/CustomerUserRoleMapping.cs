using System;

namespace BilligKwhWebApp.Core.Domain
{
    public partial class CustomerUserRoleMapping : BaseEntity
    {
        // Log
        public DateTime DateCreatedUtc { get; set; }

        public bool DefaultSelected { get; set; }

        // Foreign Keys
        public int CustomerId { get; set; }
        public int UserRoleId { get; set; }

        public CustomerUserRoleMapping ToggleDefaultSelected()
        {
            DefaultSelected = !DefaultSelected;
            return this;
        }
    }
}
