using System.Net;
using System.Threading.Tasks;

namespace GpodderLib
{
    public class GpodderClient
    {
        private readonly string _applicationName;
        private string _userAgent;

        public GpodderClient(string applicationName)
        {
            _applicationName = applicationName;
        }

        public async Task<bool> LoginAsync()
        {
            if (!await Init())
                return false;

            return true;
        }

        private async Task<bool> Init()
        {
            _userAgent = _applicationName + " (GpodderLib)";

            var req = WebRequest.CreateHttp(StaticConfiguration.ClientConfigUri);
            
#if (WP80)
            req.Headers[HttpRequestHeader.UserAgent] = _userAgent;
            var res = (HttpWebResponse) await Task.Factory.FromAsync(req.BeginGetResponse, ar => req.EndGetResponse(ar), null);
#endif

#if (NET45)
            req.UserAgent = _userAgent;
            var res = (HttpWebResponse) await req.GetResponseAsync();
#endif
            if (res.StatusCode != HttpStatusCode.OK)
                return false;

            return true;
        }
    }
}

