namespace SpectruMineAPI.Controllers
{
    namespace HardcoreDTO
    {
        public record UserStats(string Username, long LastServerTime, long TimeOnServer, List<Stats> Deaths, bool isRespawningNow);
        public record Stats(string DeathIssue, string DeathIssuer, long DeathTime, long TimeToRespawn);
        public record Top(string username, int deaths, long lastDeathtime, long timeOnServer, long lastTimeOnServer);
    }
}
