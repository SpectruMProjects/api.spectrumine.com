using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

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
      emailMessage.To.Add(new MailboxAddress("", subject));
      emailMessage.Subject = "Подтверждение регистрации";
      emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
      {
        Text = CreateHtml(
          "Подтверждение регистрации",
          "Рады приветсвовать вас на нашей платформе. Чтобы подтвердить регистрацию нажмите на кнопку",
          "htts://dev.spectrumine.com/auth/activate-register/" + code
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
        Text = CreateHtml(
          "Смена пароля",
          "Создан запрос для измения пароля на проекте SpectruMine. Чтобы подтвердить смену пароля нажмите на кнопку",
          "htts://dev.spectrumine.com/auth/activate-change-pass/" + code
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

    private string CreateHtml(string title, string text, string link)
    {
      return "<!DOCTYPE html>" +
              "<html lang=\"en\">" +
              "<head>" +
                "<meta charset=\"UTF-8\">" +
                "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">" +
                "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">" +
                "<title>" + title + "</title>" +
              "</head>" +
              "<body>" +
                "<style>" +
                  "h1 {text-align: center;}" +
                  "button {background-color: #73d13d;padding: 1em 2em;}" +
                  "button a {font-size: large;color: white;text-decoration: none;}" +
                  "p {padding: 1em;}" +
                  ".button-container {display: flex;align-items: center;justify-content: center;}" +
                "</style>" +
                "<h1>" + title + "</h1>" +
                "<p>" + text + "</p>" +
                "<div class=\"button-container\">" +
                  "<button>" +
                    "<a href=\"" + link + "\">" + "Подтвердить" + "</a>" +
                  "</button>" +
                "</div>" +
              "</body>" +
              "</html>";
    }
  }
}