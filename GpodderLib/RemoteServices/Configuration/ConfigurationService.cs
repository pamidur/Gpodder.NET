using System;
using System.Threading.Tasks;
using GpodderLib.RemoteServices.Configuration.Dto;

namespace GpodderLib.RemoteServices.Configuration
{
    internal class ConfigurationService : RemoteServiceBase
    {
        public async Task<ClientConfig> QueryClientConfig()
        {
            var uri = new Uri(StaticConfiguration.ClientConfigUri);
            return await Query<ClientConfig>(uri);
        }

        public async Task UpdateClientConfig(bool force = false)
        {
            if (DynamicConfiguration.ClientConfigData == null ||
                DynamicConfiguration.LastClientConfigSync.AddSeconds(DynamicConfiguration.ClientConfigData.UpdateTimeout) > DateTimeOffset.UtcNow ||
                force)
            {
                DynamicConfiguration.ClientConfigData = await QueryClientConfig();
                DynamicConfiguration.LastClientConfigSync = DateTimeOffset.Now;
            }
        }
    }
}
