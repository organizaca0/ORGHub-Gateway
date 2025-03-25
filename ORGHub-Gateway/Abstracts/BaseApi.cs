using System.Security.Claims;
using System.Text;
using MongoDB.Bson;
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

        protected List<string> Endpoints { get; } = new List<string>();

        public BaseApi(UserService userService, IHttpContextAccessor httpContextAccessor) 
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }
        public async Task<string> HandleRequest(GatewayRequest req)
        {
            var urlBuilder = new StringBuilder($"{ProjectAddress}/api/{req.ProjectId}/{req.ControllerId}");

            if (req.Parameters != null && req.Parameters.Count > 0)
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
                var httpRequest = new HttpRequestMessage(req.GetHttpMethod(), url);

                if (req.Body != null && req.SupportsBody())
                {
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(req.Body), Encoding.UTF8, "application/json");
                    httpRequest.Content = jsonContent;
                }

                HttpResponseMessage response = await httpClient.SendAsync(httpRequest);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
        }

        protected void AddEndpointRoles(string endpoint)
        {
            if (!(this.Endpoints.Contains(endpoint)))
            {
                this.Endpoints.Add(endpoint);
            }
        }

        public async Task<bool> ValidateAccess(GatewayRequest req)
        {
            string pars = req.ControllerId;

            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
                return false;

            ObjectId userId = ObjectId.Parse(user.FindFirst(ClaimTypes.SerialNumber)?.Value); 
            if (userId == null)
                return false; 

            User foundUser = await _userService.GetUserById(userId);
            List<string> allowedEndpoints = foundUser.GetEndpointsForProject(req.ProjectId);

            foreach(var par in req.Parameters)
            {
                pars += "/" + par.ToString();
            }
            return allowedEndpoints.Contains(pars);
        }
    }
}

