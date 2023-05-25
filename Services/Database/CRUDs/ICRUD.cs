using MongoDB.Driver;

namespace SpectruMineAPI.Services.Database.CRUDs
{
    public interface ICRUD<T> where T : class
    {
        public Task<List<T>> GetAsync();
        public Task<T?> GetAsync(System.Linq.Expressions.Expression<Func<T, bool>> expression);
        public Task<List<T>> GetAsync(System.Linq.Expressions.Expression<Func<T, bool>> expression, System.Linq.Expressions.Expression<Func<T, object>> sort);
        public Task CreateAsync(T entity);
        public Task UpdateAsync(string id, T entity);
        public Task DeleteAsync(string id);
        public IMongoCollection<T> GetForAggregate();
    }
}
