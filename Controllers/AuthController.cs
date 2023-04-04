using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpectruMineAPI.Controllers.AuthDTO;
using SpectruMineAPI.Controllers.AuthDTO.Public;
using SpectruMineAPI.Services.Auth;

namespace SpectruMineAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private AuthService authService;
        public AuthController(AuthService authService) => this.authService = authService;
        [HttpGet("Users")]
        public async Task<ActionResult<UsersResponse>> GetUsers()
        {
            List<Models.User> users = await authService.GetUsers();
            return new UsersResponse(users.ConvertAll(user => user.MapToAuth()));
        }
        [HttpPost("Reg")]
        public async Task<IActionResult> CreateUser(RegisterQuery query)
        {
            var status = await authService.CreateAccount(query.Username, query.Password, query.Email);
            if (status != AuthService.Errors.Success)
            {
                switch (status)
                {
                    case AuthService.Errors.RegexNotMatch:
                        return BadRequest(new Models.Error(status.ToString(), "InvalidData"));
                    case AuthService.Errors.Conflict:
                        return Conflict(new Models.Error(status.ToString(), "AccountExists"));
                    case AuthService.Errors.UUIDFailed:
                        return BadRequest(new Models.Error(status.ToString(), "FailedToGetMojangUUID"));
                }
            }
            return Ok();
        }
        [HttpPost("Tokens")]
        public async Task<ActionResult<AuthResponse>> AuthentificateUser(AuthQuery query)
        {
            var status = await authService.CheckUser(query.Username, query.Password);
            switch (status)
            {
                case AuthService.Errors.InvalidPassword:
                    return Unauthorized(new Models.Error(status.ToString(), "WrongPassword"));
                case AuthService.Errors.UserNotFound:
                    return Unauthorized(new Models.Error(status.ToString(), "UserNotExist"));
                case AuthService.Errors.AccountDisabled:
                    return Unauthorized(new Models.Error(status.ToString(), "CheckMail"));
            }
            var auth = await authService.GenerateTokens(query.Username);
            return new AuthResponse(auth.AccessToken, auth.RefreshToken.Token);
        }
        [HttpGet("GetUser")]
        [Authorize]
        public async Task<ActionResult<User>> GetUser()
        {
            var result = await authService.GetUserById(User.Identity!.Name!);
            if (result == null) return Unauthorized(new Models.Error(AuthService.Errors.UserNotFound.ToString(), "UsernameChanged"));
            return result.MapToAuth();
        }
        /// <summary>
        /// Update refreshToken method
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("UpdateToken")]
        public async Task<ActionResult<UpdateResponse>> UpdateToken(UpdateQuery query)
        {
            var status = await authService.CheckToken(query.RefreshToken);
            switch (status)
            {
                case AuthService.Errors.UserNotFound:
                    return Unauthorized(new Models.Error(status.ToString(), "TokenNotExist"));
                case AuthService.Errors.TokenExpire:
                    return Unauthorized(new Models.Error(status.ToString(), "TokenExipred"));
            }
            var response = await authService.UpdateTokens(query.RefreshToken);
            return new UpdateResponse(response.AccessToken, response.RefreshToken.Token);
        }
        /// <summary>
        /// Logout other tokens method
        /// </summary>
        [HttpPost("ReloadTokens")]
        public async Task<ActionResult<UpdateResponse>> ReloadTokens(UpdateQuery query)
        {
            var status = await authService.CheckToken(query.RefreshToken);
            switch (status)
            {
                case AuthService.Errors.UserNotFound:
                    return Unauthorized(new Models.Error(status.ToString(), "TokenNotExist"));
                case AuthService.Errors.TokenExpire:
                    return Unauthorized(new Models.Error(status.ToString(), "TokenExipred"));
            }
            var response = await authService.UpdateTokens(query.RefreshToken);
            return new UpdateResponse(response.AccessToken, response.RefreshToken.Token);
        }

        /// <summary>
        /// Logout current token method
        /// </summary>
        [HttpPost("Logout")]
        public async Task<ActionResult> Logout(UpdateQuery query)
        {
            var status = await authService.CheckToken(query.RefreshToken);
            switch (status)
            {
                case AuthService.Errors.UserNotFound:
                    return Unauthorized(new Models.Error(status.ToString(), "TokenNotExist"));
                case AuthService.Errors.TokenExpire:
                    return Unauthorized(new Models.Error(status.ToString(), "TokenExipred"));
            }
            authService.RemoveToken(query.RefreshToken);
            return Ok();
        }
        [HttpPost("ResetPassword")]
        public async Task<ActionResult> ResetPass(ResetPassQuery query)
        {
            var status = await authService.UpdatePassword(query.Email, query.NewPassword);
            switch (status)
            {
                case AuthService.Errors.UserNotFound:
                    return NotFound(new Models.Error(status.ToString(), "IncorrectMail"));
                case AuthService.Errors.RegexNotMatch:
                    return BadRequest(new Models.Error(status.ToString(), "IncorrectData"));
            }
            return Ok();
        }
        [Authorize]
        [HttpPost("ResetPasswordAuth")]
        public async Task<ActionResult> ResetPassAuth(ResetPassQueryAuth query)
        {
            var userMail = await authService.GetMailByUsername(User.Identity!.Name!);
            if (userMail == null) return BadRequest(new Models.Error("Null", "Каким образом это вообще могло возникнуть? Ты долбаёб?"));
            var status = await authService.UpdatePassword(userMail, query.NewPassword);
            switch (status)
            {
                case AuthService.Errors.UserNotFound:
                    return NotFound(new Models.Error(status.ToString(), "IncorrectMail"));
                case AuthService.Errors.RegexNotMatch:
                    return BadRequest(new Models.Error(status.ToString(), "IncorrectData"));
            }
            return Ok();
        }
    }
}
