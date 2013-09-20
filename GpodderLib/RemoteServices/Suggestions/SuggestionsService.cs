using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GpodderLib.LocalServices;
using GpodderLib.RemoteServices.Authentication;
using GpodderLib.RemoteServices.Configuration;
using GpodderLib.RemoteServices.Suggestions.Dto;

namespace GpodderLib.RemoteServices.Suggestions
{
    public class SuggestionsService : SecuredRemoteServiceBase
    {
        private const string ApiUri = "/suggestions/{count}.json";

        public SuggestionsService(
            StaticConfiguration staticConfiguration, 
            DynamicConfiguration dynamicConfiguration,
            ConfigurationService configurationService,
            AuthenticationService authenticationService)
            : base(staticConfiguration, dynamicConfiguration, configurationService, authenticationService)
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
