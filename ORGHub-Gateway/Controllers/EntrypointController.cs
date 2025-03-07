using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ORGHub_Gateway.Factories;
using ORGHub_Gateway.Models;
using ORGHub_Gateway.Validations;

namespace ORGHub_Gateway.Controllers
{
    [ApiController]
    [Authorize]
    [Route("gateway")]
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
        public async Task<IActionResult> HandleRequest([FromBody] GatewayRequest request)
        {
            var errors = ModelValidator.Validate(request);

            if (errors != null)
                return BadRequest(errors);

            try
            {
                var api = _apiFactory.GetApi(request.ProjectId);

                var result = await api.HandleRequest(request);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest("ERRO: Falha ao fazer requisição: " + ex.Message);
            }
        }
    }
}
