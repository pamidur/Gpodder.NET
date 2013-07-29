using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GpodderLib.RemoteServices.Suggestions.Dto;

namespace GpodderLib.RemoteServices.Suggestions
{
    internal class SuggestionsService : SecuredRemoteServiceBase
    {
        private const string ApiUri = "/suggestions/{count}.json";

        public async Task<SuggestionSet> QuerySuggestions(int count)
        {
            var uri = new Uri(DynamicConfiguration.ClientConfigData.ApiConfig.BaseUrl, ApiUri.Replace("{count}", count.ToString(CultureInfo.InvariantCulture)));
            return await Query<SuggestionSet>(uri);
        }
    }
}
