namespace Mango.Web.Service.IService
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, string logoPath, string codeOTP);

        Task SendEmailAsync(string email, string subject, string message, string logoPath);
    }
}
