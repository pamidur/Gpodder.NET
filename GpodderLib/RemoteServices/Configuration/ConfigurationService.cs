using System;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using GpodderLib.RemoteServices.Configuration.Dto;

namespace GpodderLib.RemoteServices.Configuration
{
    class ConfigurationService
    {
        private readonly DataContractJsonSerializer _serializer = new DataContractJsonSerializer(typeof(ClientConfig));
        private readonly RemoteServiceBase _requestFactory;
        private readonly StaticConfiguration _staticConfiguration;

        public ConfigurationService()
        {
            _requestFactory = ServiceLocator.Instance.GetService<RemoteServiceBase>();
            _staticConfiguration = ServiceLocator.Instance.GetService<StaticConfiguration>();
        }

        public async Task<ClientConfig> QueryClientConfig()
        {
            var request = _requestFactory.CreateRequest(new Uri(_staticConfiguration.ClientConfigUri));

#if (WP80)
            var res = (HttpWebResponse) await Task.Factory.FromAsync(request.BeginGetResponse, ar => request.EndGetResponse(ar), null);
#endif
#if (NET45)
            var res = (HttpWebResponse) await request.GetResponseAsync();
#endif
            //if (res.StatusCode != HttpStatusCode.OK)
            //    return false;

            var configuration = (ClientConfig) _serializer.ReadObject(res.GetResponseStream());

            return configuration;
        }
    }
}
