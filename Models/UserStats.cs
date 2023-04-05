using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SpectruMineAPI.Models
{
    public class UserStats
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } = string.Empty;
        public long lastServerTime { get; set; }
        public long timeOnServer { get; set; }
        public string username { get; set; } = string.Empty;
        public bool isRespawningNow { get; set; }
        public List<Stats> stats { get; set; } = new List<Stats>();
    }
    public class Stats
    {
        public string deathIssue { get; set; } = string.Empty;
        public string deathIssuer { get; set; } = string.Empty;
        public long deathTime { get; set; }
        public long timeToRespawn { get; set; }
    }
}
