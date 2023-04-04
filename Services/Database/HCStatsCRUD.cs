using MongoDB.Driver;
using SpectruMineAPI.Models;

namespace SpectruMineAPI.Services.Database
{
    public class HCStatsCRUD
    {
        private readonly IMongoCollection<UserStats> mongoCollection;
        public HCStatsCRUD(MongoService service)
        {
            mongoCollection = service.HCStatsCollection;
        }
        public async Task<List<UserStats>> GetAsync() =>
            await mongoCollection.Find(_ => true).ToListAsync();
        public async Task<UserStats?> GetAsync(System.Linq.Expressions.Expression<Func<UserStats, bool>> expression) =>
            await mongoCollection.Find(expression).FirstOrDefaultAsync();
        public async Task CreateAsync(UserStats newUser) =>
            await mongoCollection.InsertOneAsync(newUser);
        public async Task UpdateAsync(string id, UserStats updatedUserStats) =>
            await mongoCollection.ReplaceOneAsync(x => x._id == id, updatedUserStats);
        public async Task RemoveAsync(string id) =>
            await mongoCollection.DeleteOneAsync(x => x._id == id);
    }
}
