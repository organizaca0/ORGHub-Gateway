using Microsoft.AspNetCore.Mvc;
using ORGHub_Gateway.Models;
using ORGHub_Gateway.Services;
using ORGHub_Gateway.Validations;

namespace ORGHub_Gateway.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly UserService _userService;
        private readonly AuthService _authService;

        public AuthController(UserService userService, AuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticationRequest authReq)
        {
            var errors = ModelValidator.Validate(authReq);

            if (errors != null)
                return BadRequest(errors);

            var username = authReq.Username;

            if (await _userService.IsLocked(username))
            {
                await _userService.RefreshLastLogin(username);
                return StatusCode(401, "Tente novamente mais tarde.");
            }

            
            var res = await _authService.Authenticate(authReq);
            if (res != null)
            {
                await _userService.RefreshLastLogin(username);
                await _userService.ResetAttempts(username);
                return Ok(res);
            }
            else
            {
                await _userService.RefreshLastLogin(username);
                await _userService.UpdateAttempts(username);
                return BadRequest("Usuário ou senha incorretos, tente novamente.");
            }
        }
    }
}
