using System.Text;
using Newtonsoft.Json;
using ORGHub_Gateway.Enums;
using ORGHub_Gateway.Models;

namespace ORGHub_Gateway.Abstracts
{
    public abstract class BaseApi
    {
        public abstract string ProjectName { get; }
        public abstract string ProjectAddress { get; }

        protected Dictionary<string, List<Role>> EndpointRole { get; } = new Dictionary<string, List<Role>>();


        public async Task<string> HandleRequest(GatewayRequest req)
        {
            // TODO separar pra um metodo separado que so trata isso "TreatRequestErrors(req)"
            if (string.IsNullOrEmpty(req.ProjectId) || string.IsNullOrEmpty(req.ControllerId) || req.HttpMethod == null)
            {
                throw new ArgumentException("ProjectId, ControllerId, and HttpMethod are required.");
            }

            var urlBuilder = new StringBuilder($"{ProjectAddress}/api/{req.ProjectId}/{req.ControllerId}");

            if (req.Parameters != null && req.Parameters.Length > 0)
            {
                foreach (var param in req.Parameters)
                {
                    urlBuilder.Append($"/{param}");
                }
            }

            if (req.QueryParams != null && req.QueryParams.Count > 0)
            {
                urlBuilder.Append("?");
                var queryParams = req.QueryParams.Select(kvp => $"{kvp.Key}={kvp.Value}");
                urlBuilder.Append(string.Join("&", queryParams));
            }

            string url = urlBuilder.ToString();

            using (var httpClient = new HttpClient())
            {
                var httpRequest = new HttpRequestMessage(req.HttpMethod, url);

                if (req.Body != null)
                {
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(req.Body), Encoding.UTF8, "application/json");
                    httpRequest.Content = jsonContent;
                }

                HttpResponseMessage response = await httpClient.SendAsync(httpRequest);

                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
        }

        protected void AddEndpointRoles(string endpoint, List<Role> Role)
        {
            if (!EndpointRole.ContainsKey(endpoint))
            {
                EndpointRole[endpoint] = new List<Role>();
            }
            EndpointRole[endpoint].AddRange(Role);
        }

        public bool IsRoleAllowed(string endpoint, Role role)
        {
            if (EndpointRole.TryGetValue(endpoint, out var allowedRole))
            {
                return allowedRole.Contains(role);
            }
            return false;
        }

        public void PrintEndpointRole()
        {
            Console.WriteLine($"Endpoint-Role Mappings for {ProjectName} ({ProjectAddress}):");
            foreach (var keyValue in EndpointRole)
            {
                Console.WriteLine($"{keyValue.Key}: {string.Join(", ", keyValue.Value)}");
            }
        }

        public async Task<bool> ValidateAcess(GatewayRequest req)
        {
            // Implementar busca pelos projects que o User tem acesso
            // validar roles dentro do project, checar se bate com os endpoints que o usuario deseja acessar
            return true;
        }
    }
}

