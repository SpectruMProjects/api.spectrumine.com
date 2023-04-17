using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SpectruMineAPI.Models;

namespace SpectruMineAPI.Services.Database.CRUDs
{
    public class UserCRUD: ICRUD<User>
    {
        private readonly IMongoCollection<User> mongoCollection;
        public UserCRUD(MongoService service)
        {
            mongoCollection = service.UserCollection;
        }
        public async Task<List<User>> GetAsync() =>
            await mongoCollection.Find(_ => true).ToListAsync();
        public async Task<User?> GetAsync(System.Linq.Expressions.Expression<Func<User, bool>> expression) =>
            await mongoCollection.Find(expression).FirstOrDefaultAsync();
        public async Task CreateAsync(User entity) =>
            await mongoCollection.InsertOneAsync(entity);
        public async Task UpdateAsync(string id, User entity) =>
            await mongoCollection.ReplaceOneAsync(x => x.Id == id, entity);
        public async Task DeleteAsync(string id) =>
            await mongoCollection.DeleteOneAsync(x => x.Id == id);
    }
}
