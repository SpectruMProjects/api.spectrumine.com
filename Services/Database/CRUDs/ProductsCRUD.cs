using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SpectruMineAPI.Models;

namespace SpectruMineAPI.Services.Database.CRUDs
{
    public class ProductsCRUD: ICRUD<Product>
    {
        private readonly IMongoCollection<Product> mongoCollection;
        public ProductsCRUD(MongoService service)
        {
            mongoCollection = service.ProductCollection;
        }
        public async Task<List<Product>> GetAsync() =>
            await mongoCollection.Find(_ => true).ToListAsync();
        public async Task<Product?> GetAsync(System.Linq.Expressions.Expression<Func<Product, bool>> expression) =>
            await mongoCollection.Find(expression).FirstOrDefaultAsync();
        public async Task CreateAsync(Product entity) =>
            await mongoCollection.InsertOneAsync(entity);
        public async Task UpdateAsync(string id, Product entity) =>
            await mongoCollection.ReplaceOneAsync(x => x.Id == id, entity);
        public async Task DeleteAsync(string id) =>
            await mongoCollection.DeleteOneAsync(x => x.Id == id);
    }
}
