using System;
using System.Net;

namespace GpodderLib
{
    public class HttpRequestFactory
    {
        private readonly string _userAgent;

        public HttpRequestFactory(string applicationName)
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
    }
}
