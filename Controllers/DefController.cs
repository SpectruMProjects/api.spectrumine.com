using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SpectruMineAPI.Controllers
{
    [Route("/")]
    [ApiController]
    public class DefController : ControllerBase
    {
        [HttpGet("/")]
        public ActionResult Index()
        {
            return Ok($"Version: {System.Reflection.Assembly.GetEntryAssembly()!.GetName().Version}");
        }
    }
}
