namespace BilligKwhWebApp.Models
{
    public class UserEditModel : BaseModel
    {
        public int CustomerId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
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
