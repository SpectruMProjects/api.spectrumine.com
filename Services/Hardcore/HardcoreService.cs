using SpectruMineAPI.Services.Database;

namespace SpectruMineAPI.Services.Hardcore
{
    public class HardcoreService
    {
        private UserCRUD Users;
        public HardcoreService(UserCRUD Users) => this.Users = Users;

        public async Task<Errors> CheckAccess(string username)
        {
            var user = await Users.GetAsync(x => x._username == username.ToLower());
            if (user == null) return Errors.UserNotFound;
            if (!user.Verified) return Errors.NoAccess;
            return Errors.Success;
        }
        public enum Errors { UserNotFound, NoAccess, Success }
    }
}
