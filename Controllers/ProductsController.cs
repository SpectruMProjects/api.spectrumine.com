using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult> GetCategory(string category)
        {
            return Ok(await ProductsService.GetProductsAsync(category));
        }
        [Authorize]
        [HttpGet("GetUserInventory")]
        public async Task<ActionResult> GetInventory()
        {
            var inventory = await ProductsService.GetInventoryById(User.Identity!.Name!);
            if (inventory == null) return BadRequest(new Models.Error("UserNotFound", "UserDoesNotExist"));
            return Ok(inventory);
        }
        [HttpPost("Debug/CreateRandProduct")]
        public async Task<ActionResult> CreateRand()
        {
            await ProductsService.CreateProduct();
            return Ok();
        }
        [HttpPost("Debug/CreateProduct")]
        public async Task<ActionResult> CreateRand(CreateProductDTO query)
        {
            await ProductsService.CreateProduct(query.Name, query.Description, query.Category, query.ImgUrl, query.ObjUrl, query.MatUrl, query.Price);
            return Ok();
        }
    }
}
