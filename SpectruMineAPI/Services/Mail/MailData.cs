namespace SpectruMineAPI.Services.Mail
{
    public class MailData
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string SMTPAddr { get; set; } = null!;
        public int SMTPPort { get; set; }
        public bool SSL { get; set; }
    }
}
