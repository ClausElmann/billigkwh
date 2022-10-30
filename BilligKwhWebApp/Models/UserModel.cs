using BilligKwhWebApp.Core.Domain;
using System;
using System.Text.Json.Serialization;

namespace BilligKwhWebApp.Models
{
    public class UserModel : BaseModel
    {
        public int CustomerId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public int LanguageId { get; set; }
        public bool Deleted { get; set; }
        public int CountryId { get; set; }
        public string TimezoneId { get; set; }
        public long? ResetPhone { get; set; }
        public bool IsSuperAdmin { get; set; }

        /// <summary>
        /// If impersonation is active, then this will be a reference to the org. user who is impersonating THIS user
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public UserModel ImpersonatingUser { get; set; }

        public UserModel()
        {

        }

        public UserModel(User entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            CustomerId = entity.CustomerId;
            Email = entity.Email;
            Name = entity.Name;
            Phone = entity.Phone;
            LanguageId = entity.LanguageId;
            Deleted = entity.Deleted;
            CountryId = entity.CountryId;
            TimezoneId = entity.TimezoneId;
            ResetPhone = entity.ResetPhone;
            IsSuperAdmin = entity.CustomerId == 1;
        }
    }

}
