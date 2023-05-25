using SpectruMineAPI.Services.Database.CRUDs;
using SpectruMineAPI.Models;
using SpectruMineAPI.Services.Hardcore;
using MongoDB.Bson;

namespace SpectruMineAPI.Services.Cache
{
    public class CacheService
    {
        private ICRUD<UserStats> Stats;
        private List<UserStats> Precached = null!;
        public List<UserStats> CachedList { get; private set; } = null!;
        ILogger<CacheService> Logger;
        public CacheService(HCStatsCRUD Stats, ILogger<CacheService> logger)
        {
            this.Stats = Stats;
            Logger = logger;
        }
    }
}
