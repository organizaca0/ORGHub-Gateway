using ORGHub_Gateway.Abstracts;
using ORGHub_Gateway.Apis;

namespace ORGHub_Gateway.Factories
{
    public class ApiFactory
    {
        private readonly Dictionary<string, Func<BaseApi>> _apiCreators;
        private readonly Dictionary<string, BaseApi> _apiInstances;

        public ApiFactory()
        {
            _apiCreators = new Dictionary<string, Func<BaseApi>>
        {
            { "orgrow", () => new OrgGrowApi() }
        };

            _apiInstances = new Dictionary<string, BaseApi>();
        }

        public BaseApi GetApi(string projectId)
        {
            if (_apiInstances.TryGetValue(projectId, out var cachedInstance))
            {
                return cachedInstance;
            }

            if (_apiCreators.TryGetValue(projectId, out var apiCreator))
            {
                var newInstance = apiCreator();
                _apiInstances[projectId] = newInstance; 
                return newInstance;
            }

            throw new ArgumentException($"No API found for project name: {projectId}");
        }
    }
}
