using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace Starkit.Services
{
    public class MailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new 
                MailboxAddress(
                    "Администрация сайта",
                    "asdoomn@yandex.kz")
            );
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using var client = new SmtpClient();
            
            await client.ConnectAsync("smtp.yandex.kz", 25, false);
            await client.AuthenticateAsync("asdoomn@yandex.kz", "");
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
            
        }
    }
}