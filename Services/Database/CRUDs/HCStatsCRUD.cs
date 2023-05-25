using MongoDB.Bson;
using MongoDB.Driver;
using SpectruMineAPI.Models;

namespace SpectruMineAPI.Services.Database.CRUDs
{
    public class HCStatsCRUD : ICRUD<UserStats>
    {
        private readonly IMongoCollection<UserStats> mongoCollection;
        public HCStatsCRUD(MongoService service)
        {
            mongoCollection = service.HCStatsCollection;
        }
        public async Task<List<UserStats>> GetAsync() =>
            await mongoCollection.Find(_ => true).ToListAsync();
        public async Task<UserStats?> GetAsync(
            System.Linq.Expressions.Expression<Func<UserStats, bool>> expression) =>
            await mongoCollection.Find(expression).FirstOrDefaultAsync();
        public async Task<List<UserStats>> GetAsync(
            System.Linq.Expressions.Expression<Func<UserStats, bool>> expression,
            System.Linq.Expressions.Expression<Func<UserStats, object>>? sorting)
        {
            var sortdef = Builders<UserStats>.Sort.Ascending(sorting);
            return await mongoCollection.Find(expression).Sort(sortdef).ToListAsync();
        }
        public async Task CreateAsync(UserStats entity) =>
            await mongoCollection.InsertOneAsync(entity);
        public async Task UpdateAsync(string id, UserStats entity) =>
            await mongoCollection.ReplaceOneAsync(x => x._id == id, entity);
        public async Task DeleteAsync(string id) =>
            await mongoCollection.DeleteOneAsync(x => x._id == id);
        public IMongoCollection<UserStats> GetForAggregate()
        {
            return mongoCollection;
        }
    }
}
