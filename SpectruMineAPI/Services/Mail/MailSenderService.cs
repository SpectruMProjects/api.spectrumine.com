using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace SpectruMineAPI.Services.Mail
{
    public class MailSenderService
    {
        private MailData mailData;
        public MailSenderService(IOptions<MailData> mailData)
        {
            this.mailData = mailData.Value;
        }
        public async void SendMessageActivate(string subject, string code)
        {
            using var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Администрация сайта", mailData.Login));
            emailMessage.To.Add(new MailboxAddress("", subject));
            emailMessage.Subject = "helloworld";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = code
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(mailData.SMTPAddr, mailData.SMTPPort, mailData.SSL);
                await client.AuthenticateAsync(mailData.Login, mailData.Password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
        public void SendMessageRestore(string subject)
        {

        }
    }
}
