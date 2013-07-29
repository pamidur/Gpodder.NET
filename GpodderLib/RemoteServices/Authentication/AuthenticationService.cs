using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GpodderLib.RemoteServices.Authentication
{
    class AuthenticationService : RemoteServiceBase
    {
        private const string ApiUri = "/api/2/auth/{username}/login.json";

        public async Task<bool> Login(string userName, string password)
        {
            var authUri = new Uri(DynamicConfiguration.ClientConfigData.ApiConfig.BaseUrl, ApiUri.Replace("{username}", userName));

            var credentialsStr = userName + ":" + password;
            var credentials64Str = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentialsStr));

            var authRequest = CreateRequest(authUri);

            authRequest.CookieContainer = DynamicConfiguration.ClientSession;
            authRequest.Method = "POST";
            authRequest.Headers.Add("Authorization", "Basic " + credentials64Str);

#if (WP80)
            var response = (HttpWebResponse) await Task.Factory.FromAsync(authRequest.BeginGetResponse, ar => authRequest.EndGetResponse(ar), null);
#endif
#if (NET45)
            var response = (HttpWebResponse)await authRequest.GetResponseAsync();
#endif

            return true;
        }

        public void CheckLogin()
        {
            Coockie  

            || (
                ["sessionid"].Expired))
            {
                
            }
        }

        public bool IsLoogedIn
        {
            get
            {
                if (DynamicConfiguration.ClientSession.Count == 0)
                    return false;

                var coockiesForApi =
                    DynamicConfiguration.ClientSession.GetCookies(
                        DynamicConfiguration.ClientConfigData.ApiConfig.BaseUrl);
            }
        }
    }
}
