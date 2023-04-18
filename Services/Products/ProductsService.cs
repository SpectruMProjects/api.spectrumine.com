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
        public async Task CreateProduct()
        {
            await Products.CreateAsync(new Product()
            {
                Name = "Hat" + new Random(((int)DateTimeOffset.Now.ToUnixTimeSeconds())).Next(),
                Description = "HatDiscr",
                Category = "hardcore",
                ImgUrl = "/images/logo.png",
                ObjUrl = "/models/test_hat.obj",
                MatUrl = "/textures/tast_hat.mat",
                Price = 100
            });
        }
        public async Task CreateProduct(string Name, string Description, string Category, string ImgUrl, string ObjUrl, string MatUrl, float Price)
        {
            await Products.CreateAsync(new Product()
            {
                Name = Name,
                Description = Description,
                Category = Category,
                ImgUrl = ImgUrl,
                ObjUrl = ObjUrl,
                MatUrl = MatUrl,
                Price = Price
            });
        }
    }
}
