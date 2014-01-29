using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GpodderLib.Dto;
using GpodderLib.Services.Base;

namespace GpodderLib.Services
{
    public class DevicesService : SecuredRemoteServiceBase
    {
        private const string ApiListUri = "/api/2/devices/{username}.json";
        private const string ApiConfigurationUri = "/api/2/devices/{username}/{device-id}.json";
        private const string ApiGetUpdatesUri = "/api/2/updates/{username}/{device-id}.json?since={timestamp}&include_actions=true";

        public DevicesService(
            Configuration configuration,
            ConfigurationService configurationService,
            AuthenticationService authenticationService) 
            : base(configuration, configurationService, authenticationService)
        {
        }

        public async Task<List<KnownDeviceInfo>> QueryDevices()
        {
            var configData = await ConfigurationService.GetClientConfig();
            var uri = new Uri(configData.ApiConfig.BaseUrl, FillInUriShortcups(ApiListUri));
            return await Query <List<KnownDeviceInfo>>(uri);
        }

        public async Task ConfigureDevice(DeviceInfo deviceInfo, string deviceId)
        {
            var configData = await ConfigurationService.GetClientConfig();
            var uri = new Uri(configData.ApiConfig.BaseUrl, ApiConfigurationUri
                .Replace("{device-id}", deviceId)
                .Replace("{username}", Configuration.Username)
                );
          
            await Query(uri, deviceInfo);
        }

        
        public async Task<DeviceUpdateInfo> GetUpdates(string deviceId, DateTimeOffset sinceTimestamp)
        {
            var configData = await ConfigurationService.GetClientConfig();
            var uri = new Uri(configData.ApiConfig.BaseUrl, ApiGetUpdatesUri
                .Replace("{device-id}", deviceId)
                .Replace("{username}", Configuration.Username)
                .Replace("{timestamp}", "0")
                );
          
            return await Query<DeviceUpdateInfo>(uri);
        }
    }
}
