using SpectruMineAPI.Models;
using SpectruMineAPI.Services.Auth;
using SpectruMineAPI.Services.Database.CRUDs;

namespace SpectruMineAPI.Services.Mail
{
    public class MailService
    {
        private ICRUD<User> Users;
        private readonly AuthService AuthService;
        public MailService(UserCRUD users, AuthService authService)
        {
            Users = users;
            AuthService = authService;
        }
        public async Task<Errors> CheckActivateUser(string code)
        {
            var users = await Users.GetAsync();
            var user = users.FirstOrDefault(x => x.MailCodes.FirstOrDefault(y => y.Code == code && !y.isRestore) != null);
            if (user != null)
            {
                var conflictuser = await Users.GetAsync(x => x.Email == user.Email && x.Verified == true);
                if(conflictuser != null)
                {
                    return Errors.UserNotFound;
                }
                var findcode = user.MailCodes.FirstOrDefault(y => y.Code == code && !y.isRestore);
                //user.MailCodes.Remove(findcode!);
                if (findcode!.ExpireAt < DateTime.UtcNow)
                {
                    await Users.UpdateAsync(user.Id, user);
                    return Errors.CodeExpire;
                }
                //user.Verified = true;
                //await Users.UpdateAsync(user.Id, user);
                return Errors.Success;
            }
            return Errors.UserNotFound;
        }
        public async Task<AuthService.Tokens> ActivateUser(string code)
        {
            var users = await Users.GetAsync();
            var user = users.FirstOrDefault(x => x.MailCodes.FirstOrDefault(y => y.Code == code && !y.isRestore) != null);
            var findcode = user!.MailCodes.FirstOrDefault(y => y.Code == code && !y.isRestore);
            user.MailCodes.Remove(findcode!);
            user.Verified = true;
            await Users.UpdateAsync(user.Id, user);
            return await AuthService.GenerateTokens(user._username);
        }
        public async Task<Errors> ActivatePassword(string code)
        {
            var users = await Users.GetAsync();
            var user = users.FirstOrDefault(x => x.MailCodes.FirstOrDefault(y => y.Code == code && y.isRestore) != null);
            if (user != null)
            {
                var findcode = user.MailCodes.FirstOrDefault(y => y.Code == code && y.isRestore);
                user.MailCodes.Remove(findcode!);
                if (findcode!.ExpireAt < DateTime.UtcNow)
                {
                    await Users.UpdateAsync(user.Id, user);
                    return Errors.CodeExpire;
                }
                user.Password = user.NewPassword!;
                user.NewPassword = null;
                await Users.UpdateAsync(user.Id, user);
                return Errors.Success;
            }
            return Errors.UserNotFound;
        }
        public enum Errors { Success, UserNotFound, CodeExpire }
    }
}
