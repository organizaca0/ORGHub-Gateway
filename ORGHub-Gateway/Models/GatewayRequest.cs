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

        [Required(ErrorMessage = "HttpRequestMethod is required.")]
        [JsonProperty("httpRequestMethod")]
        public string HttpRequestMethod { get; set; } 

        [JsonProperty("body")]
        public string? Body { get; set; }

        [JsonProperty("parameters")]
        public List<string> Parameters { get; set; } = new List<string>();

        [ValidQueryParams(ErrorMessage = "Query parameters cannot have keys without values.")]
        [JsonProperty("queryParams")]
        public Dictionary<string, object>? QueryParams { get; set; }

        public HttpMethod GetHttpMethod()
        {
            return HttpRequestMethod?.ToUpper() switch
            {
                "GET" => HttpMethod.Get,
                "POST" => HttpMethod.Post,
                "PUT" => HttpMethod.Put,
                "DELETE" => HttpMethod.Delete,
                "PATCH" => HttpMethod.Patch,
                _ => throw new ArgumentException($"Invalid HTTP method: {HttpRequestMethod}")
            };
        }

        public bool SupportsBody()
        {
            return GetHttpMethod() == HttpMethod.Post || GetHttpMethod() == HttpMethod.Put || GetHttpMethod() == HttpMethod.Patch;
        }
    }
}
