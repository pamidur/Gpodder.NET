using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GpodderLib.LocalServices;
using GpodderLib.RemoteServices.Configuration;

namespace GpodderLib.RemoteServices.Authentication
{
    public class AuthenticationService : ConfigurableRemoteServiceBase
    {
        private readonly string _userName;
        private readonly string _password;
        private const string ApiUri = "/api/2/auth/{username}/login.json";


        public AuthenticationService(
            StaticConfiguration staticConfiguration,
            DynamicConfiguration dynamicConfiguration,
            ConfigurationService configurationService,
            string userName,
            string password)
            : base(staticConfiguration, dynamicConfiguration, configurationService)
        {
            _userName = userName;
            _password = password;

            Init().Wait();
        }

        public async Task<bool> Login()
        {
            var configData = await ConfigurationService.GetClientConfig();

            var authUri = new Uri(configData.ApiConfig.BaseUrl,
                                  ApiUri.Replace("{username}", DynamicConfiguration.Username));

            var credentialsStr = DynamicConfiguration.Username + ":" + DynamicConfiguration.Password;
            var credentials64Str = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentialsStr));

            var authRequest = await CreateRequest(authUri);

            authRequest.CookieContainer = DynamicConfiguration.ClientSession;
            authRequest.Method = "POST";
            authRequest.Headers.Add("Authorization", "Basic " + credentials64Str);

#if (WP80)
            var response = (HttpWebResponse) await Task.Factory.FromAsync(authRequest.BeginGetResponse, ar => authRequest.EndGetResponse(ar), null);
#endif
#if (NET45)
            var response = (HttpWebResponse) await authRequest.GetResponseAsync();
#endif

            var coockiesForApi = DynamicConfiguration.ClientSession.GetCookies(configData.ApiConfig.BaseUrl);
            DynamicConfiguration.SessionId = coockiesForApi["sessionid"];

            return true;
        }

        private async Task Init()
        {
            if (DynamicConfiguration.Username != _userName ||
                        DynamicConfiguration.Password != _password)
                DynamicConfiguration.SessionId = null;

            DynamicConfiguration.Username = _userName;
            DynamicConfiguration.Password = _password;

            DynamicConfiguration.ClientSession = new CookieContainer();

            if (DynamicConfiguration.SessionId != null)
                DynamicConfiguration.ClientSession.Add(
                    (await ConfigurationService.GetClientConfig()).ApiConfig.BaseUrl,
                    DynamicConfiguration.SessionId);
        }
    }
}
