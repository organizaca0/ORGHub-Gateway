using System.ComponentModel.DataAnnotations;
using ORGHub_Gateway.Validations;

namespace ORGHub_Gateway.Models
{
    public class GatewayRequest
    {
        [Required(ErrorMessage = "ProjectId is required.")]
        public string ProjectId { get; set; }

        [Required(ErrorMessage = "ControllerId is required.")]
        public string ControllerId { get; set; }

        [Required(ErrorMessage = "HttpMethod is required.")]
        public HttpMethod HttpMethod { get; set; }

        public object? Body { get; set; }

        public string[]? Parameters { get; set; }

        [ValidQueryParams(ErrorMessage = "Query parameters cannot have keys without values.")]
        public Dictionary<string, string>? QueryParams { get; set; }
    }
}
