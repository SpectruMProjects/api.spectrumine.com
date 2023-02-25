using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpectruMineAPI.Controllers.DTO;
using SpectruMineAPI.Services.Auth;

namespace SpectruMineAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private AuthService authService;
        public AuthController(AuthService authService) => this.authService = authService;
        [HttpGet("users")] 
        public async Task<ActionResult<UsersResponse>> GetUsers()
        {
            List<Models.User> users = await authService.GetUsers();
            return new UsersResponse(users);
        }
        [HttpPost("reg")]
        public async Task<IActionResult> CreateUser(RegisterQuery query)
        {
            var status = await authService.CreateAccount(query.Username, query.Password, query.Email);
            if(status != AuthService.Errors.Success)
            {
                switch (status)
                {
                    case AuthService.Errors.RegexNotMatch:
                        return BadRequest(new Models.Error(status.ToString(), "InvalidData"));
                    case AuthService.Errors.Conflict:
                        return Conflict(new Models.Error(status.ToString(), "AccountExists"));
                }
            }
            return Ok();
        }
        [HttpPost("tokens")]
        public async Task<ActionResult<AuthResponse>> AuthentificateUser(AuthQuery query)
        {
            var status = await authService.CheckUser(query.Username, query.Password);
            switch (status)
            {
                case AuthService.Errors.InvalidPassword:
                    return Unauthorized(new Models.Error(status.ToString(), "WrongPassword"));
                case AuthService.Errors.UserNotFound:
                    return Unauthorized(new Models.Error(status.ToString(), "UserNotExist"));
            }
            var auth = await authService.GenerateTokens(query.Username);
            return new AuthResponse(auth.AccessToken, auth.RefreshToken.Token);
        }
        [HttpGet("checkToken")]
        [Authorize]
        public ActionResult CheckToken()
        {
            return Ok(User.Identity!.Name);
        }
    }
}
