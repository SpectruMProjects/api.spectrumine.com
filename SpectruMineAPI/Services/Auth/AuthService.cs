using Microsoft.IdentityModel.Tokens;
using SpectruMineAPI.Models;
using SpectruMineAPI.Services.Database;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace SpectruMineAPI.Services.Auth
{
    public class AuthService
    {
        private UserCRUD Users;
        public AuthService(UserCRUD users)
        {
            Users = users;
        }
        public async Task<Errors> CreateAccount(string Username, string Password, string Email)
        {
            var regexUsername = new Regex(@"^[a-zA-Z0-9_]{3,16}$");
            var regexPassword = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,32}$");
            var regexMail = new Regex("^[a-zA-Z0-9.!#$%&’*+=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\\.[a-zA-Z0-9-]+)*$");
            //Секция проверки данных
            if (!regexUsername.IsMatch(Username) || !regexPassword.IsMatch(Password) || !regexMail.IsMatch(Email))
            {
                return Errors.RegexNotMatch;
            }
            //Секция проверки существования другого аккаунта
            var user = await Users.GetAsync(x => x._username == Username.ToLower());
            if (user != null)
            {
                return Errors.Conflict;
            }
            await Users.CreateAsync(new()
            {
                Username = Username,
                _username = Username.ToLower(),
                Password = Crypto.CalculateSHA256(Password),
                Email = Email
            }); ;
            return Errors.Success;
        }
        public async Task<Errors> CheckUser(string Username, string Password)
        {
            var user = await Users.GetAsync(x => x._username == Username.ToLower());
            if (user == null) return Errors.UserNotFound;
            if (user.Password != Crypto.CalculateSHA256(Password)) return Errors.InvalidPassword;
            if (!user.verified) return Errors.AccountDisabled;
            return Errors.Success;
        }
        public async Task<Errors> CheckToken(string refreshToken)
        {
            var userList = await Users.GetAsync();
            var user = userList.FirstOrDefault(x => x.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken) != null);
            if (user == null) return Errors.UserNotFound;
            var Token = user.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken);
            if (Token!.ExpireAt < DateTime.UtcNow)
            {
                user.RefreshTokens.Remove(Token);
                await Users.UpdateAsync(user.Id, user);
                return Errors.TokenExpire;
            }
            return Errors.Success;
        }
        public async Task<Tokens> GenerateTokens(string Username)
        {
            var user = await Users.GetAsync(x => x._username == Username.ToLower());
            if (user == null) throw new ArgumentNullException($"{nameof(user)} returned null");
            var refreshToken = new RefreshToken()
            {
                Token = Crypto.CalculateSHA256(DateTime.UtcNow.ToString()),
                ExpireAt = DateTime.UtcNow.AddDays(30)
            };
            var accessToken = Crypto.GetAccessToken(user._username);
            user.RefreshTokens.Add(refreshToken);
            await Users.UpdateAsync(user.Id, user);
            return new(accessToken, refreshToken);
        }
        public async Task<Tokens> UpdateTokens(string refreshToken)
        {
            var userList = await Users.GetAsync();
            var user = userList.FirstOrDefault(x => x.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken) != null);
            if (user == null) throw new ArgumentNullException($"{nameof(user)} returned null");
            var newToken = new RefreshToken()
            {
                Token = Crypto.CalculateSHA256(DateTime.UtcNow.ToString()),
                ExpireAt = DateTime.UtcNow.AddDays(30)
            };
            var accessToken = Crypto.GetAccessToken(user._username);
            user.RefreshTokens.Remove(user.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken)!);
            user.RefreshTokens.Add(newToken);
            await Users.UpdateAsync(user.Id, user);
            return new(accessToken, newToken);
        }
        public async Task<Tokens> RemoveTokens(string refreshToken)
        {
            var userList = await Users.GetAsync();
            var user = userList.FirstOrDefault(x => x.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken) != null);
            if (user == null) throw new ArgumentNullException($"{nameof(user)} returned null");
            user.RefreshTokens.Clear();
            var newToken = new RefreshToken()
            {
                Token = Crypto.CalculateSHA256(DateTime.UtcNow.ToString()),
                ExpireAt = DateTime.UtcNow.AddDays(30)
            };
            var accessToken = Crypto.GetAccessToken(user._username);
            user.RefreshTokens.Remove(user.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken)!);
            user.RefreshTokens.Add(newToken);
            await Users.UpdateAsync(user.Id, user);
            return new(accessToken, newToken);
        }
        public async void RemoveToken(string refreshToken)
        {
            var userList = await Users.GetAsync();
            var user = userList.FirstOrDefault(x => x.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken) != null);
            if (user == null) throw new ArgumentNullException($"{nameof(user)} returned null");
            user.RefreshTokens.Remove(user.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken)!);
            await Users.UpdateAsync(user.Id, user);
        }
        public async Task<List<User>> GetUsers()
        {
            return await Users.GetAsync();
        }
        public enum Errors { RegexNotMatch, Success, Conflict, UserNotFound, InvalidPassword, TokenExpire, AccountDisabled }
        public record Tokens(string AccessToken, RefreshToken RefreshToken);
    }
    static class Crypto
    {
        public static string CalculateSHA256(string data)
        {
            return Convert.ToHexString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(data))).ToLower();
        }
        public static string GetAccessToken(string username)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow, //Действует 5 минут
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
