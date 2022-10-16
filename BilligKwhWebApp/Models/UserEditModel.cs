namespace BilligKwhWebApp.Models
{
    public class UserEditModel : BaseModel
    {
        public int CustomerId { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public int LanguageId { get; set; }
        public int CountryId { get; set; }
        public string NewPassword { get; set; }
        public bool Administrator { get; set; }

        public bool Deleted { get; set; }

        public UserEditModel()
        {

        }
    }
}
