namespace BilligKwhWebApp.Services
{
    public interface IErrorEmailSender
    {
     bool SendErrorMail(string subject, string body);
    }
}
