using System;

namespace BilligKwhWebApp.Services
{
    public class Index_User
    {
        // User Info
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int CountryId { get; set; }
        public bool Deleted { get; set; }
        public DateTime LastLoggedIn { get; set; }
        // Customer Info
        public string Customer { get; set; }
        public int CustomerId { get; set; }
    }
}
