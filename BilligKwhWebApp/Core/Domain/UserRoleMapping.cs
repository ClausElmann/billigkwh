namespace BilligKwhWebApp.Core.Domain
{
    public class UserRoleMapping : BaseEntity
    {
        // Junction Id
        public int UserId { get; set; }
        public virtual Bruger User { get; set; }

        public int UserRoleId { get; set; }
        public virtual UserRole UserRole { get; set; }

        public int CustomerId { get; set; }

        // Ctor's 
        public UserRoleMapping()
        {

        }
    }
}
