using SpectruMineAPI.Models;

namespace SpectruMineAPI.Controllers
{
    static class Mapper
    {
        public static AuthDTO.Public.User MapToAuth(this User User)
        {
            return new(User.Id, User.Username, User.Email, User.Verified);
        }
        public static MailDTO.Public.User MapToMail(this User User)
        {
            return new(User.Id, User.Username, User.Email, User.Verified);
        }
    }
}
