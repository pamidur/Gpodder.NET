using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading;
using System.Threading.Tasks;
using GpodderLib.Utils;

namespace GpodderLib.Services.Base
{
    public abstract class RemoteServiceBase
    {
        private readonly HttpClient _apiClient = new HttpClient(new HttpClientHandler { UseCookies = false }, true);
        private readonly ProductInfoHeaderValue _userAgent;

        protected Configuration Configuration { get; private set; }

        protected RemoteServiceBase(Configuration configuration)
        {
            Configuration = configuration;
            _userAgent = new ProductInfoHeaderValue(Configuration.DeviceId + "GpodderLib","1.0");
        }

        protected string FillInUriShortcups(string input)
        {
            var output = input.Replace("{username}", Configuration.Username);
            output = output.Replace("{device-id}", Configuration.DeviceId);
            return output;
        }

        protected virtual Task<HttpRequestMessage> CreateRequest(Uri uri, object argument = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() =>
                {
                    var request = new HttpRequestMessage
                        {
                            RequestUri = uri,
                            Method = argument == null ? HttpMethod.Get : HttpMethod.Post,
                            Content = argument == null ? null : new JsonContent(argument)
                        };


                    request.Headers.Connection.Add("Keep-Alive");
                    request.Headers.UserAgent.Add(_userAgent);

                    return request;
                });
        }

        protected virtual async Task<TR> Query<TR>(Uri uri)
            where TR : class
        {
            return await Query<object, TR>(uri, null);
        }

        protected virtual async Task Query<TA>(Uri uri, TA argument)
            where TA : class
        {
            await Query<TA, object>(uri, argument);
        }


        protected async Task<TR> Query<TA, TR>(Uri uri, TA argument, CancellationToken cancellationToken = default(CancellationToken))
            where TA : class 
            where TR : class 
        {
            var request = await CreateRequest(uri, argument);

#if (DEBUG)
            //Console.Write("Requesting " + uri + " : ");
#endif
            
            var responseSerializer = new DataContractJsonSerializer(typeof (TR));

            var response = await SendRequest(request, cancellationToken);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return default(TR);

                throw new Exception(response.ReasonPhrase);
            }

            var responseStream = await response.Content.ReadAsStreamAsync();

#if (DEBUG)

            var debugMemoryStream = new MemoryStream();
            responseStream.CopyTo(debugMemoryStream);
            debugMemoryStream.Seek(0, SeekOrigin.Begin);
            var a = new StreamReader(debugMemoryStream).ReadToEnd();
            debugMemoryStream.Seek(0, SeekOrigin.Begin);
            responseStream = debugMemoryStream;
            //Console.WriteLine("Done.");

#endif

            var result = /*response.ContentLength > 0
                             ? */ (TR) responseSerializer.ReadObject(responseStream)
                /* : default(TR)*/;

            return result;
        }

        protected async Task<HttpResponseMessage> SendRequest(HttpRequestMessage message, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _apiClient.SendAsync(message, cancellationToken);
        }
    }
}
