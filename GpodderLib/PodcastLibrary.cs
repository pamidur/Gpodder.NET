using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using GpodderLib.RemoteServices;
using GpodderLib.RemoteServices.Configuration;

namespace GpodderLib
{
    public class PodcastLibrary
    {
        private readonly string _applicationName;
        private readonly Stream _configurationData;

        private bool _initialized;

        private DynamicConfiguration Configuration { get; set; }

        public PodcastLibrary(string applicationName, Stream configurationData)
        {
            _applicationName = applicationName;

            _configurationData = configurationData;

            if (!_configurationData.CanRead || !_configurationData.CanWrite || !_configurationData.CanSeek)
                throw new ArgumentException(
                    "Configuration data stream should be able to be read, written and sought over.");
        }

        

        public async Task<bool> Login()
        {
            CheckInitialized();

            return true;
        }

        public async Task Init()
        {
            var loadConfigTask = LoadDynamicConfiguration();

            ServiceLocator.Instance.RegisterService(typeof (StaticConfiguration), new StaticConfiguration());

            Configuration = await loadConfigTask;
            Configuration.DeviceId = _applicationName;
            ServiceLocator.Instance.RegisterService(typeof (DynamicConfiguration), Configuration);

            var bootstrapTasks = new List<Task>
                {
                    RegisterRemoteServices(),
                    RegisterLocalServices()
                };

            await Task.WhenAll(bootstrapTasks);
            _initialized = true;
        }

        private async Task RegisterLocalServices()
        {
            
        }

        private async Task RegisterRemoteServices()
        {
            var configService = new ConfigurationService();
            ServiceLocator.Instance.RegisterService(typeof(ConfigurationService), configService);

            var updateClientConfigTask = configService.UpdateClientConfig();

            //register other services

            await updateClientConfigTask;
        }

        private void CheckInitialized()
        {
            if(!_initialized)
                throw new InvalidOperationException("Try Init() it first.");
        }

        private Task<DynamicConfiguration> LoadDynamicConfiguration()
        {
            return Task.Run(() =>
                {
                    var serializer = new DataContractJsonSerializer(typeof (DynamicConfiguration));
                    _configurationData.Seek(0, SeekOrigin.Begin);

                    try
                    {
                        return (DynamicConfiguration)serializer.ReadObject(_configurationData);
                    }
                    catch
                    {
                        return new DynamicConfiguration();
                    }

                });
        }
    }
}

