using ORGHub_Gateway.Abstracts;
using ORGHub_Gateway.Apis;

namespace ORGHub_Gateway.Factories
{
    public class ApiFactory
    {
        private readonly Dictionary<string, Func<BaseApi>> _apiCreators;

        public ApiFactory()
        {
            _apiCreators = new Dictionary<string, Func<BaseApi>>
            {
                { "orghub", () => new OrgHubApi() }
            };
        }

        public BaseApi GetApi(string projectName)
        {
            if (_apiCreators.TryGetValue(projectName, out var api))
            {
                return api();
            }
            throw new ArgumentException($"No API found for project name: {projectName}");
        }
    }
}
