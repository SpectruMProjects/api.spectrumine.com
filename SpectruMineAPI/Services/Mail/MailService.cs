using SpectruMineAPI.Services.Auth;
using SpectruMineAPI.Services.Database;

namespace SpectruMineAPI.Services.Mail
{
    public class MailService
    {
        private UserCRUD Users;
        public MailService(UserCRUD users) => Users = users;
        public async Task<Errors> ActivateUser(string code)
        {
            var users = await Users.GetAsync();
            var user = users.FirstOrDefault(x => x.MailCodes.FirstOrDefault(y => y.Code == code && !y.isRestore) != null);
            if(user != null)
            {
                var findcode = user.MailCodes.FirstOrDefault(y => y.Code == code && !y.isRestore);
                user.MailCodes.Remove(findcode!);
                if (findcode!.ExpireAt < DateTime.UtcNow)
                {
                    await Users.UpdateAsync(user.Id, user);
                    return Errors.CodeExpire;
                }
                user.Verified = true;
                await Users.UpdateAsync(user.Id, user);
                return Errors.Success;
            }
            return Errors.UserNotFound;
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
