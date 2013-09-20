using System;
using System.Threading.Tasks;
using GpodderLib.LocalServices;
using GpodderLib.RemoteServices.Authentication;
using GpodderLib.RemoteServices.Configuration;
using GpodderLib.RemoteServices.Devices.Dto;

namespace GpodderLib.RemoteServices.Devices
{
    public class DevicesService : SecuredRemoteServiceBase
    {
        private const string ApiListUri = "/api/2/devices/{username}.json";
        private const string ApiConfigurationUri = "/api/2/devices/{username}/{device-id}.json";

        public DevicesService(
            StaticConfiguration staticConfiguration,
            DynamicConfiguration dynamicConfiguration,
            ConfigurationService configurationService,
            AuthenticationService authenticationService) 
            : base(staticConfiguration, dynamicConfiguration, configurationService, authenticationService)
        {
        }

        public async Task<DeviceSet> QueryDevices()
        {
            var configData = await ConfigurationService.GetClientConfig();
            var uri = new Uri(configData.ApiConfig.BaseUrl, FillInUriShortcups(ApiListUri));
            return await Query<DeviceSet>(uri);
        }

        //public async Task RegisterDevice(string caption)
        //{
        //    var configData = await ConfigurationService.GetClientConfig();
        //    var uri = new Uri(configData.ApiConfig.BaseUrl, FillInUriShortcups(ApiConfigurationUri));
        //    return await Query<DeviceSet>(uri);
        //}
    }
}
