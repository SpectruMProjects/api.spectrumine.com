using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpectruMineAPI.Services.Products;

namespace SpectruMineAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly ProductsService ProductsService;
        public ProductsController(ProductsService productsService) => ProductsService = productsService;
        [HttpGet("{category}")]
        public async Task<IActionResult> GetCategory(string category)
        {
            return Ok(await ProductsService.GetProductsAsync(category));
        }
    }
}
