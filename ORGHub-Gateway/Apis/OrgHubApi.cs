using ORGHub_Gateway.Abstracts;
using ORGHub_Gateway.Enums;

namespace ORGHub_Gateway.Apis
{
    public class OrgHubApi : BaseApi
    {
        public override string ProjectName => "orghub";
        public override string ProjectAddress => "localhost:4001";

        public OrgHubApi()
        {
            AddEndpointRoles("/api/auth/login", [Role.None]);

            AddEndpointRoles("/api/orgrow/create", [Role.Admin]);
            AddEndpointRoles("/api/grow/delete/**", [Role.Admin]);
            AddEndpointRoles("/api/grow/change-status/**", [Role.Admin]);
            AddEndpointRoles("/api/secret/**", [Role.Admin]);
            AddEndpointRoles("/api/user/**", [Role.Admin]);
            AddEndpointRoles("/api/grow/create", [Role.Admin]);

            AddEndpointRoles("/api/enviroment/send-data", [Role.None]);
            AddEndpointRoles("/api/enviroment/setup", [Role.None]);
            AddEndpointRoles("/api/enviroment/get-data/stream", [Role.None]);

            AddEndpointRoles("/api/enviroment/get-setup", [Role.User]);
            AddEndpointRoles("/api/enviroment", [Role.User]);
            AddEndpointRoles("/api/grow/grow", [Role.User]);
            AddEndpointRoles("/api/grow/grows", [Role.User]);
            AddEndpointRoles("/api/grow/update/**", [Role.User]);
        }
    }
}
