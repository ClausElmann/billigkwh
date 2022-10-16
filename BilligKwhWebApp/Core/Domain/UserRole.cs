using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class UserRole : BaseEntity
    {
        // Props
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>
        public DateTime? DateCreatedUtc { get; set; }
        public int UserRoleCategoryId { get; set; }

        // Ctor
        public UserRole()
        {

        }
    }
}
