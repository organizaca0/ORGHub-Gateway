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

            AddEndpointRoles("/grow/create", [Role.Admin]);
            AddEndpointRoles("/grow/delete", [Role.Admin]);
            AddEndpointRoles("/grow/change-status", [Role.Admin]);
            AddEndpointRoles("/grow/grow", [Role.User]);
            AddEndpointRoles("/grow/grows", [Role.User]);
            AddEndpointRoles("/grow/update", [Role.User]);

            AddEndpointRoles("/enviroment/send-data", [Role.None]);
            AddEndpointRoles("/enviroment/setup", [Role.None]);
            AddEndpointRoles("/enviroment/get-data/stream", [Role.None]);
            AddEndpointRoles("/enviroment/get-setup", [Role.User]);
            AddEndpointRoles("/enviroment", [Role.User]);

            AddEndpointRoles("/secret", [Role.Admin]);
        }
    }
}
