using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using GpodderLib.RemoteServices.Configuration;
using GpodderLib.RemoteServices.Configuration.Dto;

namespace GpodderLib
{
    [DataContract]
    class DynamicConfiguration
    {
        [DataMember]
        public DateTimeOffset LastServerSync { get; set; }

        [DataMember]
        public DateTimeOffset LastClientConfigSync { get; set; }

        [DataMember]
        public ClientConfig ClientConfigData { get; set; }

        private readonly Stream _configurationData;
        private ConfigurationService _configurationService;

        public DynamicConfiguration(Stream configurationData)
        {
            _configurationData = configurationData;

            if (!_configurationData.CanRead || !_configurationData.CanWrite || !_configurationData.CanSeek)
                throw new ArgumentException(
                    "Configuration data stream should be able to be read, written and sought over.");

        }

        public void Init()
        {
            _configurationService = ServiceLocator.Instance.GetService<ConfigurationService>();
        }

        public async Task UpdateClientConfig(bool force = false)
        {
            if (ClientConfigData == null || LastClientConfigSync.AddSeconds(ClientConfigData.UpdateTimeout) > DateTimeOffset.UtcNow || force)
            {
                ClientConfigData = await _configurationService.QueryClientConfig();
            }
        }
    }
}
