using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SpectruMineAPI.Models;

namespace SpectruMineAPI.Services.Database
{
    public class UserCRUD
    {
        private readonly IMongoCollection<User> mongoCollection;
        public UserCRUD(IOptions<DBSettings> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);
            var database = mongoClient.GetDatabase(options.Value.DatabaseName);
            mongoCollection = database.GetCollection<User>("users");
        }
        public async Task<List<User>> GetAsync() =>
            await mongoCollection.Find(_ => true).ToListAsync();
        public async Task<User?> GetAsync(System.Linq.Expressions.Expression<Func<User, bool>> expression) =>
            await mongoCollection.Find(expression).FirstOrDefaultAsync();
        public async Task CreateAsync(User newUser) =>
            await mongoCollection.InsertOneAsync(newUser);
        public async Task UpdateAsync(string id, User updatedUser) =>
            await mongoCollection.ReplaceOneAsync(x => x.Id == id, updatedUser);
        public async Task RemoveAsync(string id) =>
            await mongoCollection.DeleteOneAsync(x => x.Id == id);
    }
}
