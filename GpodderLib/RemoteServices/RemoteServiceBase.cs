using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using GpodderLib.LocalServices;

namespace GpodderLib.RemoteServices
{
    public abstract class RemoteServiceBase
    {
        private readonly string _userAgent;

        protected DynamicConfiguration DynamicConfiguration { get; private set; }
        protected StaticConfiguration StaticConfiguration { get; private set; }

        protected RemoteServiceBase(StaticConfiguration staticConfiguration, DynamicConfiguration dynamicConfiguration)
        {
            DynamicConfiguration = dynamicConfiguration;
            StaticConfiguration = staticConfiguration;
            _userAgent = DynamicConfiguration.DeviceId + " (GpodderLib)";
        }

        protected string FillInUriShortcups(string input)
        {
            var output = input.Replace("{username}", DynamicConfiguration.Username);
            output = output.Replace("{device-id}", DynamicConfiguration.DeviceId);
            return output;
        }
       

        protected async Task<HttpWebRequest> CreateRequest(Uri uri, object outgoingContent = null)
        {
            var req = WebRequest.CreateHttp(uri);

#if (DEBUG)
            Console.WriteLine("Requesting " + uri);
#endif
            req.KeepAlive = true;

#if (WP80)
            req.Headers[HttpRequestHeader.UserAgent] = _userAgent;
#endif
#if (NET45)
            req.UserAgent = _userAgent;
#endif
            
            req.CookieContainer = DynamicConfiguration.ClientSession;

            if (outgoingContent != null)
            {
                req.Method = "POST";

                //req.GetRequestStream().WriteAsync()
            }

            return req;
        }

        protected virtual async Task<TR> Query<TR>(Uri uri)
        {
            return await Query<object, TR>(uri, null);
        }

        protected virtual async Task<TR> Query<TA, TR>(Uri uri, TA argument)
        {
            var request = await CreateRequest(uri);

#if (WP80)
            var response = (HttpWebResponse) await Task.Factory.FromAsync(request.BeginGetResponse, ar => request.EndGetResponse(ar), null);
#endif
#if (NET45)
            var response = (HttpWebResponse) await request.GetResponseAsync();
#endif
            //if (res.StatusCode != HttpStatusCode.OK)
            //    return false;

            
            //var a = new StreamReader(response.GetResponseStream()).ReadToEnd();
            

            var serializer = new DataContractJsonSerializer(typeof (TR));
            var result = (TR) serializer.ReadObject(response.GetResponseStream());

            return result;
        }
    }
}
