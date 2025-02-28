namespace ORGHub_Gateway.Models
{
    public class GatewayRequest
    {
        public required string ProjectId;
        public required string ControllerId;
        public required HttpMethod HttpMethod;
        public object? Body;
        public string[]? Parameters;
        public Dictionary<string, string>? QueryParams;
    }
}
