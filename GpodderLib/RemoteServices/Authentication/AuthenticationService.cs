using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GpodderLib.RemoteServices.Authentication
{
    class AuthenticationService
    {
        private const string UsernameShortcut = "{username}";
        private const string ApiUri = "/api/2/auth/" + UsernameShortcut + "/login.json";

        private RemoteServiceBase _requestFactory;
        private DynamicConfiguration _dynamicConfiguration;

        public AuthenticationService()
        {
            _requestFactory = ServiceLocator.Instance.GetService<RemoteServiceBase>();
            _dynamicConfiguration = ServiceLocator.Instance.GetService<DynamicConfiguration>();
        }

        public async Task<bool> Login(string userName, string password)
        {
            var authUri = new Uri(_dynamicConfiguration.ClientConfigData.ApiConfig.BaseUrl,
                                  ApiUri.Replace(UsernameShortcut, userName));

            var credentialsStr = userName + ":" + password;
            var credentials64Str = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentialsStr));

            var authRequest = _requestFactory.CreateRequest(authUri);

            authRequest.Method = "POST";
            authRequest.Headers.Add("Authorization","Basic " + credentials64Str);

#if (WP80)
            var response = (HttpWebResponse) await Task.Factory.FromAsync(authRequest.BeginGetResponse, ar => authRequest.EndGetResponse(ar), null);
#endif
#if (NET45)
            var response = (HttpWebResponse) await authRequest.GetResponseAsync();
#endif


        }
    }
}
