using System;
using System.Globalization;
using System.Threading.Tasks;
using GpodderLib.Dto;
using GpodderLib.Services.Base;

namespace GpodderLib.Services
{
    public class SuggestionsService : SecuredRemoteServiceBase
    {
        private const string ApiUri = "/suggestions/{count}.json";

        public SuggestionsService(
            Configuration configuration,
            ConfigurationService configurationService,
            AuthenticationService authenticationService)
            : base(configuration, configurationService, authenticationService)
        {
        }

        public async Task<SuggestionSet> QuerySuggestions(int count)
        {
            var configData = await ConfigurationService.GetClientConfig();
            var uri = new Uri(configData.ApiConfig.BaseUrl, ApiUri.Replace("{count}", count.ToString(CultureInfo.InvariantCulture)));
            return await Query<SuggestionSet>(uri);
        }
    }
}
