namespace SpectruMineAPI.Controllers
{
    namespace HardcoreDTO
    {
        public record UserStats(string Username, long LastServerTime, long TimeOnServer, List<Stats> Deaths);
        public record Stats(string DeathIssue, string DeathIssuer, long DeathTime, long TimeToRespawn);
    }
}
