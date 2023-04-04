using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SpectruMineAPI.Models;

namespace SpectruMineAPI.Services.Database
{
    public class MongoService
    {
        public readonly IMongoCollection<User> UserCollection;
        public readonly IMongoCollection<UserStats> HCStatsCollection;
        MongoClient MongoClient;
        public MongoService(IOptions<DBSettings> options)
        { 
            MongoClient = new MongoClient(options.Value.ConnectionString);
            var database = MongoClient.GetDatabase(options.Value.DatabaseName);
            UserCollection = database.GetCollection<User>("users");
            HCStatsCollection = database.GetCollection<UserStats>("hardcore-stats");
        }
    }
}
