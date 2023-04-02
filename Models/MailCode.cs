namespace SpectruMineAPI.Models
{
    public class MailCode
    {
        public string Code { get; set; } = null!;
        public DateTime ExpireAt { get; set; }
        public bool isRestore { get; set; }
    }
}
