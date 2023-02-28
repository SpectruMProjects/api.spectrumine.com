using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpectruMineAPI.Models;
using SpectruMineAPI.Services.Auth;
using SpectruMineAPI.Services.Mail;

namespace SpectruMineAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly MailService MailService;
        public MailController(MailService mailService)
        {
            MailService = mailService;
        }

        [HttpGet("activate/{key}")]
        public async Task<ActionResult> Activate(string key)
        {
            var result = await MailService.ActivateUser(key);
            switch (result)
            {
                case MailService.Errors.UserNotFound: return BadRequest(new Error(result.ToString(), "CodeNotFound"));
                case MailService.Errors.CodeExpire: return BadRequest(new Error(result.ToString(), "CodeExpired"));
            };
            return Ok();
        }
        [HttpGet("restore/{key}")]
        public async Task<ActionResult> Restore(string key)
        {
            var result = await MailService.ActivatePassword(key);
            switch (result)
            {
                case MailService.Errors.UserNotFound: return BadRequest(new Error(result.ToString(), "CodeNotFound"));
                case MailService.Errors.CodeExpire: return BadRequest(new Error(result.ToString(), "CodeExpired"));
            }
            return Ok();
        }
    }
}
