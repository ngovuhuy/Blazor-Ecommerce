using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace TangyWeb_API.Service
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> options)
        {
            _mailSettings = options.Value;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            if (mailRequest == null)
            {
                // Handle the case where mailRequest is null
                throw new ArgumentNullException(nameof(mailRequest));
            }

            if (string.IsNullOrEmpty(_mailSettings.Mail))
            {
                // Handle the case where _mailSettings.Mail is null or empty
                throw new ArgumentNullException(nameof(_mailSettings.Mail), "Mail settings not configured.");
            }

            if (string.IsNullOrEmpty(mailRequest.ToEmail))
            {
                // Handle the case where mailRequest.ToEmail is null or empty
                throw new ArgumentNullException(nameof(mailRequest.ToEmail), "ToEmail cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(mailRequest.Subject))
            {
                // Handle the case where mailRequest.Subject is null or empty
                throw new ArgumentNullException(nameof(mailRequest.Subject), "Subject cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(mailRequest.Body))
            {
                // Handle the case where mailRequest.Body is null or empty
                throw new ArgumentNullException(nameof(mailRequest.Body), "Body cannot be null or empty.");
            }

            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
