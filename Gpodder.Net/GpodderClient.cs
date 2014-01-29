using System;
using System.IO;
using System.Threading.Tasks;
using GpodderLib.Services;

namespace GpodderLib
{
    public class GpodderClient : IDisposable
    {
        private readonly Stream _configurationData;
        private readonly string _applicationName;
        private readonly string _username;
        private readonly string _password;

        public Configuration Configuration { get; protected set; }
        public ConfigurationService ConfigurationService { get; protected set; }
        public AuthenticationService AuthenticationService { get; protected set; }
        public SuggestionsService SuggestionsService { get; protected set; }
        public DevicesService DevicesService { get; protected set; }
        public DirectoryService DirectoryService { get; protected set; }

        private GpodderClient(Stream configurationData, string applicationName, string username, string password)
        {
            _configurationData = configurationData;
            _applicationName = applicationName;
            _username = username;
            _password = password;
        }

        private async Task Bootstrap()
        {
            Configuration = await Configuration.LoadFrom(_configurationData);

            ConfigurationService = new ConfigurationService(Configuration);
            AuthenticationService = new AuthenticationService(Configuration,ConfigurationService, _username, _password);
            SuggestionsService = new SuggestionsService(Configuration, ConfigurationService,AuthenticationService);
            DevicesService = new DevicesService(Configuration, ConfigurationService,AuthenticationService);
            DirectoryService = new DirectoryService(Configuration, ConfigurationService);
        }

        public static async Task<GpodderClient> Init(Stream configurationData, string applicationName, string username,
                                                     string password)
        {
            var client = new GpodderClient(configurationData, applicationName, username, password);
            await client.Bootstrap();
            return client;
        }

        public void Dispose()
        {
            Configuration.SaveTo(_configurationData).Wait();
            GC.SuppressFinalize(this);
        }

        ~GpodderClient()
        {
            Dispose();
        }
    }
}

