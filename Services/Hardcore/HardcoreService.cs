using MongoDB.Bson;
using MongoDB.Driver;
using SpectruMineAPI.Models;
using SpectruMineAPI.Services.Cache;
using SpectruMineAPI.Services.Database;
using SpectruMineAPI.Services.Database.CRUDs;

namespace SpectruMineAPI.Services.Hardcore
{
    public class TopComparer : IComparer<UserStats>
    {
        public int Compare(UserStats? userStats1, UserStats? userStats2)
        {
            if (userStats1!.timeOnServer / (userStats1!.stats.Count + 1) < userStats2!.timeOnServer / (userStats2!.stats.Count + 1))
                return 1;
            else if (userStats1!.timeOnServer / (userStats1!.stats.Count + 1) > userStats2!.timeOnServer / (userStats2!.stats.Count + 1))
                return -1;
                return 0;
        }
    }
    public class HardcoreService
    {
        private ICRUD<User> Users;
        private ICRUD<UserStats> Stats;
        private CacheService Cache;
        public HardcoreService(UserCRUD Users, HCStatsCRUD Stats, CacheService Cache)
        {
            this.Users = Users;
            this.Stats = Stats;
            this.Cache = Cache;
        }
        public async Task<List<UserStats>> GetTop10()
        {
            var res = await Stats.GetAsync();
            res.Sort(new TopComparer());
            return res;
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
