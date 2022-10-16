
namespace BilligKwhWebApp.Models
{
    public class UserRoleAccessModel
    {
        public UserRoleModel UserRole { get; set; }
        public bool HasAccess { get; set; }
        public bool DefaultSelected { get; set; }
    }
}
