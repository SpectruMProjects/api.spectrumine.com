namespace SpectruMineAPI.Controllers
{
    namespace MailDTO
    {
        namespace Public
        {
            public record User(string Id, string Username, string Email, bool Verified);
        }
        public record ActResponse(string AccessToken, string RefreshToken);
    }
}
