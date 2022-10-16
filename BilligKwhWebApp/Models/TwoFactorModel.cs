namespace BilligKwhWebApp.Models
{
    public class TwoFactorModel
    {
        public string Email { get; set; }
        public int PinCode { get; set; }
        public long SmsGroupId { get; set; }
    }
}
