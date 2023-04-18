using SpectruMineAPI.Models;
using SpectruMineAPI.Services.Database.CRUDs;
using static SpectruMineAPI.Controllers.Mapper;

namespace SpectruMineAPI.Services.Hardcore
{
    public class HardcoreService
    {
        private ICRUD<User> Users;
        private ICRUD<UserStats> Stats;

        public HardcoreService(UserCRUD Users, HCStatsCRUD Stats)
        {
            this.Users = Users;
            this.Stats = Stats;
        }
        public async Task<Errors> CheckAccess(string username)
        {
            var user = await Users.GetAsync(x => x._username == username.ToLower());
            if (user == null) return Errors.UserNotFound;
            if (!user.Verified) return Errors.NoAccess;
            return Errors.Success;
        }
        public async Task<Errors> CheckStatsAvailable(string username)
        {
            var stats = await Stats.GetAsync(x => x.username.ToLower() == username.ToLower());
            if(stats == null) return Errors.UserNotFound;
            return Errors.Success;
        }
        public async Task<UserStats> GetStats(string username)
        {
            var stats = (await Stats.GetAsync(x => x.username.ToLower() == username.ToLower()))!;
            return stats;
        }
        public enum Errors { UserNotFound, NoAccess, Success }

    }
}
