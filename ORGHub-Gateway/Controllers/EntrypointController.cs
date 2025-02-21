using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ORGHub_Gateway.Factories;
using ORGHub_Gateway.Models;

namespace ORGHub_Gateway.Controllers
{
    public class EntrypointController : ControllerBase
    {
        private readonly ApiFactory _apiFactory;

        public EntrypointController(ApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
        }

        [HttpGet("isAlive")]
        public IActionResult Get()
        {
            return Ok(true);
        }

        [HttpPost("request")]
        public IActionResult HandleRequest([FromBody] GatewayRequest request)
        {
            try
            {
                var api = _apiFactory.GetApi(request.ProjectName);

                var result = api.HandleRequest(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool IsRoleAllowed(string url, string role)
        {
            return true; 
        }
    }
}
