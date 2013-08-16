using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using GpodderLib.LocalServices;
using GpodderLib.RemoteServices.Configuration;

namespace GpodderLib.RemoteServices
{
    abstract class RemoteServiceBase : ServiceBase
    {
        private string _userAgent;
        protected DynamicConfigurationService DynamicConfigurationService { get; private set; }
        protected StaticConfigurationService StaticConfigurationService { get; private set; }
        protected ConfigurationService ConfigurationService { get; private set; }

        public override async Task Init()
        {
            await base.Init();

            StaticConfigurationService = ServiceLocator.Get<StaticConfigurationService>();
            DynamicConfigurationService = ServiceLocator.Get<DynamicConfigurationService>();

            ConfigurationService = ServiceLocator.Get<ConfigurationService>();

            _userAgent = DynamicConfigurationService.DeviceId + " (GpodderLib)";
        }

        protected string FillInUriShortcups(string input)
        {
            var output = input.Replace("{username}", DynamicConfigurationService.Username);
            output = output.Replace("{device-id}", DynamicConfigurationService.DeviceId);
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
            
            req.Headers.Add(AppendAdditionalHeader());
            req.CookieContainer = DynamicConfigurationService.ClientSession;

            if (outgoingContent != null)
            {
                req.Method = "POST";

                //req.GetRequestStream().WriteAsync()
            }

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
