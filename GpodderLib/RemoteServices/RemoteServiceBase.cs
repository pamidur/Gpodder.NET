using System;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace GpodderLib.RemoteServices
{
    public class RemoteServiceBase
    {
        private readonly string _userAgent;

        public RemoteServiceBase(string applicationName)
        {
            _userAgent = applicationName + " (GpodderLib)";
        }

        public HttpWebRequest CreateRequest(Uri uri)
        {
            var req = WebRequest.CreateHttp(uri);
            req.KeepAlive = true;

#if (WP80)
            req.Headers[HttpRequestHeader.UserAgent] = _userAgent;
#endif
#if (NET45)
            req.UserAgent = _userAgent;
#endif

            return req;
        }



        public async Task<TR> Query<TA, TR>(Uri uri, TA argument)
        {
            var request = CreateRequest(uri);

#if (WP80)
            var response = (HttpWebResponse) await Task.Factory.FromAsync(request.BeginGetResponse, ar => request.EndGetResponse(ar), null);
#endif
#if (NET45)
            var response = (HttpWebResponse) await request.GetResponseAsync();
#endif
            //if (res.StatusCode != HttpStatusCode.OK)
            //    return false;

            var serializer = new DataContractJsonSerializer(typeof (TR));
            var result = (TR) serializer.ReadObject(response.GetResponseStream());

            return result;
        }
    }
}
