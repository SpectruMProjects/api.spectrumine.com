using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SpectruMineAPI.Services.Auth
{
    public class AuthOptions
    {
        public const string ISSUER = "spectrum"; // издатель токена
        public const string AUDIENCE = "client"; // потребитель токена
        public static string KEY = null!;   // ключ для шифрации
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
        public static bool UseMail = true; //Активировать через почту?
        public static bool UseMojangChecks = true; //Mojang UUID get
    }
}
