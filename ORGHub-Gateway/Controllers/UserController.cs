using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ORGHub_Gateway.Models;
using ORGHub_Gateway.Services;
using ORGHub_Gateway.Validations;

namespace ORGHub_Gateway.Controllers
{
    [Route("user")]
    [Authorize]
    public class UserController : Controller
    {

        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            var errors = ModelValidator.Validate(user);

            if (errors != null)
                return BadRequest(errors);

            if (await _userService.CreateUser(user))
            {
                return StatusCode(StatusCodes.Status201Created);
            }
            else
            {
                return BadRequest("Usuário já existente");
            }
        }
    }
}
