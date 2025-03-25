using System.Security.Claims;
using ORGHub_Gateway.Abstracts;
using ORGHub_Gateway.Enums;
using ORGHub_Gateway.Interfaces;
using ORGHub_Gateway.Services;

namespace ORGHub_Gateway.Apis
{
    public class OrgGrowApi : BaseApi
    {
        public override string ProjectName => "orgrow";
        public override string ProjectAddress => "http://localhost:4001";

        private readonly UserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrgGrowApi(UserService userService, IHttpContextAccessor httpContextAccessor)
            : base(userService, httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user.FindFirst(ClaimTypes.SerialNumber)?.Value;

            AddEndpointRoles("grow/create");
            AddEndpointRoles("grow/delete");
            AddEndpointRoles("grow/change-status");
            AddEndpointRoles("grow/grow");
            AddEndpointRoles($"grow/all/{userId}");
            AddEndpointRoles("grow/update");

            AddEndpointRoles("enviroment/send-data");
            AddEndpointRoles("enviroment/setup");
            AddEndpointRoles("enviroment/get-data/stream");
            AddEndpointRoles("enviroment/get-setup");
            AddEndpointRoles("enviroment");

            AddEndpointRoles("secret");
        }
    }
}
