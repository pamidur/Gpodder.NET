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
    public class GpodderClient : IDisposable
    {
        private readonly Stream _configurationData;
        private readonly string _applicationName;
        private readonly string _username;
        private readonly string _password;

        public StaticConfiguration StaticConfiguration { get; protected set; }
        public DynamicConfiguration DynamicConfiguration { get; protected set; }
        public ConfigurationService ConfigurationService { get; protected set; }
        public AuthenticationService AuthenticationService { get; protected set; }
        public SuggestionsService SuggestionsService { get; protected set; }
        public DevicesService DevicesService { get; protected set; }

        private GpodderClient(Stream configurationData, string applicationName, string username, string password)
        {
            _configurationData = configurationData;
            _applicationName = applicationName;
            _username = username;
            _password = password;
        }

        private async Task Bootstrap()
        {
            StaticConfiguration = new StaticConfiguration();
            DynamicConfiguration = await DynamicConfiguration.LoadFrom(_configurationData);

            ConfigurationService = new ConfigurationService(StaticConfiguration, DynamicConfiguration);

            AuthenticationService = new AuthenticationService(StaticConfiguration, DynamicConfiguration,
                ConfigurationService, _username, _password);

            SuggestionsService = new SuggestionsService(StaticConfiguration, DynamicConfiguration, ConfigurationService,
                AuthenticationService);

            DevicesService = new DevicesService(StaticConfiguration, DynamicConfiguration, ConfigurationService,
                AuthenticationService);
        }

        public void Dispose()
        {
            DynamicConfiguration.SaveTo(_configurationData).Wait();
            GC.SuppressFinalize(this);
        }

        public static async Task<GpodderClient> Init(Stream configurationData, string applicationName, string username,
                                                      string password)
        {
            var lib = new GpodderClient(configurationData, applicationName, username, password);
            await lib.Bootstrap();
            return lib;
        }

        ~GpodderClient()
        {
            Dispose();
        }
    }
}

