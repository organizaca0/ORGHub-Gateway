using ORGHub_Gateway.Models;

namespace ORGHub_Gateway.Abstracts
{
    public abstract class BaseApi
    {
        public abstract string ProjectName { get; }
        public abstract string ProjectAddress { get; }

        protected Dictionary<string, List<string>> EndpointRoles { get; } = new Dictionary<string, List<string>>();


        public bool HandleRequest(GatewayRequest req)
        {
            return true;
        }

        protected void AddEndpointRoles(string endpoint, params string[] roles)
        {
            if (!EndpointRoles.ContainsKey(endpoint))
            {
                EndpointRoles[endpoint] = new List<string>();
            }
            EndpointRoles[endpoint].AddRange(roles);
        }

        public bool IsRoleAllowed(string endpoint, string role)
        {
            if (EndpointRoles.TryGetValue(endpoint, out var allowedRoles))
            {
                return allowedRoles.Contains(role);
            }
            return false;
        }

        public void PrintEndpointRoles()
        {
            Console.WriteLine($"Endpoint-Role Mappings for {ProjectName} ({ProjectAddress}):");
            foreach (var keyValue in EndpointRoles)
            {
                Console.WriteLine($"{keyValue.Key}: {string.Join(", ", keyValue.Value)}");
            }
        }
    }
}

