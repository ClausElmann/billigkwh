namespace BilligKwhWebApp.Core.Dto
{
    public class RefreshTokenDto
    {
        public string RefreshToken { get; set; }
        public int ProfileId { get; set; }
        public long SmsGroupId { get; set; }
        public int UserId { get; set; }
    }
}
