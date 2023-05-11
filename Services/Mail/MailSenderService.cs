using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Razor.Templating.Core;

namespace SpectruMineAPI.Services.Mail
{
    public class MailSenderService
    {
        private MailData mailData;
        private ILogger<MailSenderService> Logger;

        public MailSenderService(IOptions<MailData> mailData, ILogger<MailSenderService> logger)
        {
            this.mailData = mailData.Value;
            this.Logger = logger;
        }
        public async void SendMessageActivate(string subject, string code)
        {
            if (!Services.Auth.AuthOptions.UseMail)
            {
                Logger.LogInformation($"Отправка кода активации {subject} {code}");
                return;
            }
            using var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Администрация сайта", mailData.Login));
            emailMessage.To.Add(new MailboxAddress(subject, subject));
            emailMessage.Subject = "Подтверждение регистрации";
            var body = new BodyBuilder();
            body.HtmlBody = await CreateHtml(
                "Подтверждение регистрации",
                "Рады приветствовать вас на нашей платформе. Чтобы подтвердить регистрацию нажмите на кнопку",
                "https://dev.spectrumine.com/auth/activate-register/" + code
                );
            emailMessage.Body = body.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(mailData.SMTPAddr, mailData.SMTPPort, mailData.SSL);
                await client.AuthenticateAsync(mailData.Login, mailData.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
        public async void SendMessageRestore(string subject, string code)
        {
            if (!Services.Auth.AuthOptions.UseMail)
            {
                Logger.LogInformation($"Отправка кода активации {subject} {code}");
                return;
            }

            using var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Администрация сайта", mailData.Login));
            emailMessage.To.Add(new MailboxAddress("", subject));
            emailMessage.Subject = "helloworld";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = await CreateHtml(
                "Смена пароля",
                "Создан запрос для изменения пароля на проекте SpectruMine. Чтобы подтвердить смену пароля нажмите на кнопку",
                "https://dev.spectrumine.com/auth/activate-change-pass/" + code
                )
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(mailData.SMTPAddr, mailData.SMTPPort, mailData.SSL);
                await client.AuthenticateAsync(mailData.Login, mailData.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
        private async Task<string> CreateHtml(string title, string text, string link)
        {
            return await RazorTemplateEngine.RenderAsync("~/Pages/mail.cshtml", new TemplateData()
            {
                title = title,
                text = text,
                link = link
            });
        }
        //private string CreateHtml(string title, string text, string link)
        //{
        //    return $"""
        //        <h1 style="text-align: center"> {title} на SpectruMine.com</h1>
        //        <table width=100% border=0 cellspacing=0 cellpadding=0>
        //            <tr>
        //                <td align="center">
        //                    <p style="padding: 1em"> {text} </p>
        //                </td>
        //            </tr>
        //            <tr>
        //                <td align="center">
        //                    <button style="background-color: #73d13d;padding: 1em 2em">
        //                        <a style="font-size: large;color: white;text-decoration: none;" href="{link}"> Подтвердить </a>
        //                    </button>
        //                </td>
        //            </tr>
        //            <tr>
        //                <td align="center">
        //                    <p style="padding: 1em">Если кнопка не работает, перейдите по этой ссылке:
        //                        <a href="{link}"> {link} </a>
        //                    </p>
        //                </td>
        //            </tr>
        //            <tr>
        //                <td align="center">
        //                    <p style="padding: 1em">
        //                        Если это сообщение отправлено по ошибке или вы не совершали никаких действий на платформе
        //                        spectrumine.com - проигнорируйте это письмо
        //                    </p>
        //                </td>
        //            </tr>
        //        </table>
        //        """;
        //}
    }
}