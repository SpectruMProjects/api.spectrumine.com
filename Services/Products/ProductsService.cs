using SpectruMineAPI.Models;
using SpectruMineAPI.Services.Database.CRUDs;

namespace SpectruMineAPI.Services.Products
{
    public class ProductsService
    {
        //TODO: Change CRUD to ICRUD (Add method with List<T> GetAsyncList
        readonly ProductsCRUD Products;
        public ProductsService(ProductsCRUD products)
        { 
            Products = products;
        }
        public async Task<List<Product>> GetProductsAsync(string category)
        {
            var products = await Products.GetAsyncList(x => x.Category == category);
            return products;
        }
    }
}
