using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GpodderLib.Services.Base;

namespace GpodderLib.Services
{
    public class AuthenticationService : ConfigurableRemoteServiceBase
    {
        private readonly string _userName;
        private readonly string _password;
        private static readonly string _apiUri = "/api/2/auth/{username}/login.json";

        public AuthenticationService(
            Configuration configuration,
            ConfigurationService configurationService,
            string userName,
            string password)
            : base(configuration, configurationService)
        {
            _userName = userName;
            _password = password;

            Init();
        }

        public Cookie SessionId
        {
            get { return Configuration.SessionId; }
            private set { Configuration.SessionId = value; }
        }

        public bool IsLoogedIn
        {
            get
            {
                if (SessionId == null || SessionId.Expired)
                    return false;

                return true;
            }
        }

        public async Task<bool> Login(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (IsLoogedIn)
                return true;

            var getConfigJob = ConfigurationService.GetClientConfig();

            var credentialsStr = Configuration.Username + ":" + Configuration.Password;
            var credentials64Str = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentialsStr));

            var authUri = new Uri((await getConfigJob).ApiConfig.BaseUrl,
                                  _apiUri.Replace("{username}", Configuration.Username));

            var authRequest = await CreateRequest(authUri);

            authRequest.Method = HttpMethod.Post;
            authRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials64Str);

            var response = await SendRequest(authRequest);

            var cookies = new CookieContainer();
            cookies.SetCookies(authUri, String.Join(", ",response.Headers.GetValues("Set-Cookie")));

            SessionId = cookies.GetCookies(new Uri("http://gpodder.net"))["sessionid"];

#if DEBUG
           // if (SessionId != null)
                //Console.WriteLine("Authenticated");
#endif

            return true;
        }

        private void Init()
        {
            if (Configuration.Username != _userName ||
                        Configuration.Password != _password)
                SessionId = null;

            if (!IsLoogedIn)
                SessionId = null;

            Configuration.Username = _userName;
            Configuration.Password = _password;
        }
    }
}
