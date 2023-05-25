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
        public static List<HardcoreDTO.Stats> Map(this List<Stats> stats)
        {
            List<HardcoreDTO.Stats> result = new List<HardcoreDTO.Stats>();
            foreach (Stats item in stats)
            {
                result.Add(new(item.deathIssue, item.deathIssuer, item.deathTime, item.timeToRespawn));
            }
            return result;
        }
        public static HardcoreDTO.UserStats Map(this UserStats stats)
        {
            return new(stats.username, stats.lastServerTime, stats.timeOnServer, stats.stats.Map(), stats.isRespawningNow);
        }
        public static HardcoreDTO.Top MapToTop(this UserStats stats)
        {
            return new(stats.username, stats.stats.Count, stats.stats.Count != 0 ? stats.stats.Last().timeToRespawn : 0, stats.timeOnServer, stats.lastServerTime);
        }
        public static List<HardcoreDTO.Top> MapToTop(this List<UserStats> stats)
        {
            List<HardcoreDTO.Top> top = new List<HardcoreDTO.Top>();
            foreach(UserStats item in stats)
            {
                top.Add(item.MapToTop());
            }
            return top;
        }
    }
}
