namespace BilligKwhWebApp.Models
{
    public class LoginEmailPasswordModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public long? SmsGroupId { get; set; }
    }
}
