using Microsoft.AspNetCore.Mvc;
using SpectruMineAPI.Controllers.MailDTO.Public;
using SpectruMineAPI.Services.Auth;
using SpectruMineAPI.Services.Mail;

namespace SpectruMineAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly MailService MailService;
        public MailController(MailService mailService, AuthService authService)
        {
            MailService = mailService;
        }

        [HttpGet("activate/{key}")]
        public async Task<ActionResult<MailDTO.ActResponse>> Activate(string key)
        {
            var result = await MailService.CheckActivateUser(key);
            switch (result)
            {
                case MailService.Errors.UserNotFound: return BadRequest(new Models.Error(result.ToString(), "CodeNotFound"));
                case MailService.Errors.CodeExpire: return BadRequest(new Models.Error(result.ToString(), "CodeExpired"));
            };
            var tokens = await MailService.ActivateUser(key);
            return new MailDTO.ActResponse(tokens.AccessToken, tokens.RefreshToken.Token);
        }
        [HttpGet("restore/{key}")]
        public async Task<ActionResult> Restore(string key)
        {
            var result = await MailService.ActivatePassword(key);
            switch (result)
            {
                case MailService.Errors.UserNotFound: return BadRequest(new Models.Error(result.ToString(), "CodeNotFound"));
                case MailService.Errors.CodeExpire: return BadRequest(new Models.Error(result.ToString(), "CodeExpired"));
            }
            return Ok();
        }
    }
}
