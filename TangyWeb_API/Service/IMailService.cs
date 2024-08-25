namespace TangyWeb_API.Service
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest  mailRequest);
    }
}
