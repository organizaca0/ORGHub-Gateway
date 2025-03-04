using Microsoft.AspNetCore.Mvc;
using ORGHub_Gateway.Models;
using ORGHub_Gateway.Services;

namespace ORGHub_Gateway.Controllers
{
    [Route("user")]
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
            try
            {
                if (await _userService.CreateUser(user))
                {
                    return StatusCode(StatusCodes.Status201Created);
                }
                else
                {
                    return BadRequest("Usuário já existente");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
