using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using GpodderLib.RemoteServices;
using GpodderLib.RemoteServices.Configuration;

namespace GpodderLib
{
    public class PodcastLibrary
    {
        private readonly string _applicationName;
        private readonly IsolatedStorageFile _storage;

        public PodcastLibrary(string applicationName, IsolatedStorageFile storage)
        {
            _applicationName = applicationName;
            _storage = storage;
        }

        public async Task<bool> Login()
        {
            if (!await Init())
                return false;

            return true;
        }

        private async Task<bool> Init()
        {
            ServiceLocator.Instance.RegisterService(typeof(StaticConfiguration), new StaticConfiguration());

            ServiceLocator.Instance.RegisterService(typeof(HttpRequestFactory), new HttpRequestFactory(_applicationName));


            ServiceLocator.Instance.RegisterService(typeof(ConfigurationService), new ConfigurationService(_storage));


            await cs.Init();

            return true;
        }
    }
}

