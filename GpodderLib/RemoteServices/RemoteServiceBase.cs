using System;
using System.Collections.Specialized;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace GpodderLib.RemoteServices
{
    abstract class RemoteServiceBase
    {
        private readonly string _userAgent;
        protected DynamicConfiguration DynamicConfiguration { get; private set; }
        protected StaticConfiguration StaticConfiguration { get; private set; }

        protected RemoteServiceBase()
        {
            StaticConfiguration = ServiceLocator.Instance.GetService<StaticConfiguration>();
            DynamicConfiguration = ServiceLocator.Instance.GetService<DynamicConfiguration>();

            _userAgent = DynamicConfiguration.DeviceId + " (GpodderLib)";
        }

        protected HttpWebRequest CreateRequest(Uri uri)
        {
            var req = WebRequest.CreateHttp(uri);
            req.KeepAlive = true;

#if (WP80)
            req.Headers[HttpRequestHeader.UserAgent] = _userAgent;
#endif
#if (NET45)
            req.UserAgent = _userAgent;
#endif
            
            req.Headers.Add(AppendAdditionalHeader());

            return req;
        }

        protected virtual NameValueCollection AppendAdditionalHeader()
        {
            return new WebHeaderCollection();
        }

        protected virtual async Task<TR> Query<TR>(Uri uri)
        {
            return await Query<object, TR>(uri, null);
        }

        protected virtual async Task<TR> Query<TA, TR>(Uri uri, TA argument)
        {
            var request = CreateRequest(uri);

            request.CookieContainer = DynamicConfiguration.ClientSession;

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
