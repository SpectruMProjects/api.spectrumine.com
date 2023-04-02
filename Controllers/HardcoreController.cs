using Microsoft.AspNetCore.Mvc;
using SpectruMineAPI.Services.Hardcore;

namespace SpectruMineAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HardcoreController : ControllerBase
    {
        private HardcoreService hardcoreService;
        public HardcoreController(HardcoreService hardcoreService) => this.hardcoreService = hardcoreService;
        /// <summary>
        /// Method for WhiteList plugin
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("access/{username}")]
        public async Task<ActionResult> CheckAccess(string username)
        {
            var result = await hardcoreService.CheckAccess(username);
            switch(result)
            {
                case HardcoreService.Errors.UserNotFound:
                    return NotFound(new Models.Error(result.ToString(), "UserNotExist"));
                case HardcoreService.Errors.NoAccess:
                    return BadRequest(new Models.Error(result.ToString(), "UserNotActivated"));
            }
            return Ok();
        }
        [HttpGet("stats/{username}")]
        public async Task<ActionResult> GetStats(string username)
        {
            return Ok();
        }
    }
}
