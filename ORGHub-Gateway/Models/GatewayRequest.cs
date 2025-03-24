using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ORGHub_Gateway.Validations;

namespace ORGHub_Gateway.Models
{
    public class GatewayRequest
    {
        [Required(ErrorMessage = "ProjectId is required.")]
        [JsonProperty("projectId")] 
        public string ProjectId { get; set; }

        [Required(ErrorMessage = "ControllerId is required.")]
        [JsonProperty("controllerId")]
        public string ControllerId { get; set; }

        [Required(ErrorMessage = "HttpMethod is required.")]
        [JsonProperty("httpMethod")] 
        public HttpMethod HttpMethod { get; set; }

        [JsonProperty("body")] 
        public object? Body { get; set; }

        [JsonProperty("parameters")] 
        public List<string> Parameters { get; set; }

        [ValidQueryParams(ErrorMessage = "Query parameters cannot have keys without values.")]
        [JsonProperty("queryParams")] 
        public Dictionary<string, object>? QueryParams { get; set; }
    }
}
