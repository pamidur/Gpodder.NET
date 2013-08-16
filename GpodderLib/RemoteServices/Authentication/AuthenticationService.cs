using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GpodderLib.RemoteServices.Configuration;

namespace GpodderLib.RemoteServices.Authentication
{
    internal class AuthenticationService : RemoteServiceBase
    {
        private readonly string _userName;
        private readonly string _password;
        private const string ApiUri = "/api/2/auth/{username}/login.json";

        protected ConfigurationService ConfigurationService { get; set; }

        public AuthenticationService(string userName, string password)
        {
            _userName = userName;
            _password = password;
        }

        public async Task<bool> Login()
        {
            var configData = await ConfigurationService.GetClientConfig();

            var authUri = new Uri(configData.ApiConfig.BaseUrl,
                                  ApiUri.Replace("{username}", DynamicConfigurationService.Username));

            var credentialsStr = DynamicConfigurationService.Username + ":" + DynamicConfigurationService.Password;
            var credentials64Str = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentialsStr));

            var authRequest = CreateRequest(authUri);

            authRequest.CookieContainer = DynamicConfigurationService.ClientSession;
            authRequest.Method = "POST";
            authRequest.Headers.Add("Authorization", "Basic " + credentials64Str);

#if (WP80)
            var response = (HttpWebResponse) await Task.Factory.FromAsync(authRequest.BeginGetResponse, ar => authRequest.EndGetResponse(ar), null);
#endif
#if (NET45)
            var response = (HttpWebResponse) await authRequest.GetResponseAsync();
#endif

            var coockiesForApi = DynamicConfigurationService.ClientSession.GetCookies(configData.ApiConfig.BaseUrl);
            DynamicConfigurationService.SessionId = coockiesForApi["sessionid"];

            return true;
        }

        public override async Task Init()
        {
            await base.Init();

            ConfigurationService = ServiceLocator.Get<ConfigurationService>();

            if (DynamicConfigurationService.Username != _userName ||
                        DynamicConfigurationService.Password != _password)
                DynamicConfigurationService.SessionId = null;

            DynamicConfigurationService.Username = _userName;
            DynamicConfigurationService.Password = _password;

            DynamicConfigurationService.ClientSession = new CookieContainer();

            if (DynamicConfigurationService.SessionId != null)
                DynamicConfigurationService.ClientSession.Add(
                    (await ConfigurationService.GetClientConfig()).ApiConfig.BaseUrl,
                    DynamicConfigurationService.SessionId);
            
        }
    }
}
