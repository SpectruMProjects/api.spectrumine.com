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
            return Ok("Version: Pre-release 03.04.2023 1:24");
        }
    }
}
