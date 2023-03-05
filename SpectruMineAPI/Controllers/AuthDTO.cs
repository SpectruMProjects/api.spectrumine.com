using SpectruMineAPI.Models;

namespace SpectruMineAPI.Controllers
{
    namespace AuthDTO
    {
        public record UsersResponse(List<User> Users);
        public record RegisterQuery(string Username, string Password, string Email);
        public record AuthQuery(string Username, string Password);
        public record AuthResponse(string AccessToken, string RefreshToken);
        public record UpdateQuery(string RefreshToken);
        public record UpdateResponse(string AccessToken, string RefreshToken);
        public record ResetPassQuery(string Email, string NewPassword);
        public record ResetPassQueryAuth(string NewPassword);
    }
}
