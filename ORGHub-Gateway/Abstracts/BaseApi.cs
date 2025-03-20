using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;
using ORGHub_Gateway.Enums;
using ORGHub_Gateway.Models;
using ORGHub_Gateway.Services;

namespace ORGHub_Gateway.Abstracts
{
    public abstract class BaseApi
    {
        public abstract string ProjectName { get; }
        public abstract string ProjectAddress { get; }

        private readonly UserService _userService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        protected Dictionary<string, List<Role>> EndpointRoles { get; } = new Dictionary<string, List<Role>>();

        public BaseApi(UserService userService, IHttpContextAccessor httpContextAccessor) 
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }
        public async Task<string> HandleRequest(GatewayRequest req)
        {
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
            if (!EndpointRoles.ContainsKey(endpoint))
            {
                EndpointRoles[endpoint] = new List<Role>();
            }
            EndpointRoles[endpoint].AddRange(Role);
        }

        public bool IsRoleAllowed(string endpoint, List<Role> roles)
        {
            return EndpointRoles.TryGetValue(endpoint, out var allowedRole) &&
                   roles.Any(role => allowedRole.Contains(role));
        }

        public async Task<bool> ValidateAccess(GatewayRequest req)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null)
                return false; 

            var username = user.FindFirst(ClaimTypes.Name)?.Value; 
            if (username == null)
                return false; 

            User foundUser = await _userService.GetUserByUsername(username);
            List<Role> userRoles = foundUser.GetRolesForProject(req.ProjectId);

            return IsRoleAllowed(req.Parameters.ToString(), userRoles);
        }
    }
}

