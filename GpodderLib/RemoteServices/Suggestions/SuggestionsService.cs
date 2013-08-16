using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GpodderLib.RemoteServices.Configuration;
using GpodderLib.RemoteServices.Suggestions.Dto;

namespace GpodderLib.RemoteServices.Suggestions
{
    internal class SuggestionsService : SecuredRemoteServiceBase
    {
        private const string ApiUri = "/suggestions/{count}.json";

        protected ConfigurationService ConfigurationService { get; set; }

        public async Task<SuggestionSet> QuerySuggestions(int count)
        {
            var configData = await ConfigurationService.GetClientConfig();
            var uri = new Uri(configData.ApiConfig.BaseUrl, ApiUri.Replace("{count}", count.ToString(CultureInfo.InvariantCulture)));
            return await Query<SuggestionSet>(uri);
        }

        public override async Task Init()
        {
            await base.Init();
            ConfigurationService = ServiceLocator.Get<ConfigurationService>();
        }
    }
}
