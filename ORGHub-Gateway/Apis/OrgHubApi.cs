using ORGHub_Gateway.Abstracts;

namespace ORGHub_Gateway.Apis
{
    public class OrgHubApi : BaseApi
    {
        public override string ProjectName => "orghub";
        public override string ProjectAddress => "localhost:4001";

        public OrgHubApi()
        {
            AddEndpointRoles("/api/orgrow/create", "ADMIN");
            AddEndpointRoles("/api/grow/delete/**", "ADMIN");
            AddEndpointRoles("/api/grow/change-status/**", "ADMIN");
            AddEndpointRoles("/api/secret/**", "ADMIN");
            AddEndpointRoles("/api/user/**", "ADMIN");

            AddEndpointRoles("/api/auth/login");
            AddEndpointRoles("/api/enviroment/send-data/**");
            AddEndpointRoles("/api/enviroment/setup/**");
            AddEndpointRoles("/api/enviroment/get-setup/**", "USER");
            AddEndpointRoles("/api/enviroment/get-data/stream/**");
            AddEndpointRoles("/api/enviroment/**", "USER");
            AddEndpointRoles("/api/grow/grow/**", "USER");
            AddEndpointRoles("/api/grow/grows", "USER");
            AddEndpointRoles("/api/grow/create", "ADMIN");
            AddEndpointRoles("/api/grow/update/**", "USER");
        }
    }
}
