using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using GpodderLib.RemoteServices;
using GpodderLib.RemoteServices.Configuration;

namespace GpodderLib
{
    public class PodcastLibrary
    {
        private readonly string _applicationName;
        private readonly Stream _configurationDataStream;

        private bool _initialized;


        public PodcastLibrary(string applicationName, Stream configurationDataStream)
        {
            _applicationName = applicationName;
            _configurationDataStream = configurationDataStream;
        }

        

        public async Task<bool> Login()
        {
            CheckInitialized();

            return true;
        }

        public async Task Init()
        {
            ServiceLocator.Instance.RegisterService(typeof(StaticConfiguration), new StaticConfiguration());
            ServiceLocator.Instance.RegisterService(typeof(DynamicConfiguration), new DynamicConfiguration(_configurationDataStream));
            ServiceLocator.Instance.RegisterService(typeof(RemoteServiceBase), new RemoteServiceBase(_applicationName));

            RegisterRemoteServices();

            _initialized = true;
        }

        private void RegisterRemoteServices()
        {
            ServiceLocator.Instance.RegisterService(typeof(ConfigurationService), new ConfigurationService());
        }
       

        private void CheckInitialized()
        {
            if(!_initialized)
                throw new InvalidOperationException("Try Init() it first.");
        }
    }
}

