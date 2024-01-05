using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System.Net.Mail;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace TangyWeb_API.Helper
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        public EmailSender(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
;
                var emailToSend = new MimeMessage();
                emailToSend.From.Add(MailboxAddress.Parse("huynvce161149@fpt.edu.vn"));
                emailToSend.To.Add(MailboxAddress.Parse(email));
                emailToSend.Subject = subject;
                emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

                //send email
                using var emailClient = new MailKit.Net.Smtp.SmtpClient();
                emailClient.Connect("smtp.email.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                emailClient.Authenticate("huynvce161149@fpt.edu.vn", "qfagihlitabmempd");
                emailClient.Send(emailToSend);
                emailClient.Disconnect(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
