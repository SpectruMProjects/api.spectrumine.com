namespace SpectruMineAPI.Models
{
    public class RefreshToken
    {
        public string Token { get; set; } = null!;
        public long ExpireAt { get; set; }
    }
}
