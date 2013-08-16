using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using GpodderLib.LocalServices;
using GpodderLib.RemoteServices;
using GpodderLib.RemoteServices.Authentication;
using GpodderLib.RemoteServices.Configuration;
using GpodderLib.RemoteServices.Devices;
using GpodderLib.RemoteServices.Devices.Dto;
using GpodderLib.RemoteServices.Suggestions;

namespace GpodderLib
{
    public class PodcastLibrary : IDisposable
    {
        private readonly ServiceLocator _serviceLocator;
        private readonly Stream _configurationData;
        private readonly string _applicationName;
        private readonly string _username;
        private readonly string _password;

        private PodcastLibrary(Stream configurationData, string applicationName, string username, string password)
        {
            _configurationData = configurationData;
            _applicationName = applicationName;
            _username = username;
            _password = password;
            _serviceLocator = new ServiceLocator();
        }

        public async Task<List<Device>> GetDevices()
        {
            return await _serviceLocator.Get<DevicesService>().QueryDevices();
        }

        private async Task Bootstrap()
        {
            _serviceLocator.RegisterService(typeof(StaticConfigurationService), new StaticConfigurationService());
            _serviceLocator.RegisterService(typeof(DynamicConfigurationService), await DynamicConfigurationService.LoadFrom(_configurationData));
            _serviceLocator.RegisterService(typeof(ConfigurationService), new ConfigurationService());
            _serviceLocator.RegisterService(typeof(AuthenticationService), new AuthenticationService(_username, _password));
            _serviceLocator.RegisterService(typeof(SuggestionsService), new SuggestionsService());
            _serviceLocator.RegisterService(typeof(DevicesService), new DevicesService());
            
            await _serviceLocator.InitServices();
        }

        public void Dispose()
        {
            _serviceLocator.Get<DynamicConfigurationService>().SaveTo(_configurationData).Wait();
            GC.SuppressFinalize(this);
        }

        public static async Task<PodcastLibrary> Init(Stream configurationData, string applicationName, string username,
                                                      string password)
        {
            var lib = new PodcastLibrary(configurationData, applicationName, username, password);
            await lib.Bootstrap();
            return lib;
        }

        ~PodcastLibrary()
        {
            Dispose();
        }
    }
}

