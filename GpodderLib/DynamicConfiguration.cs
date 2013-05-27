using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GpodderLib.RemoteServices.Configuration.Dto;

namespace GpodderLib
{
    class DynamicConfiguration
    {
        public async Task Init()
        {
            if (_storage.FileExists(StaticConfiguration.ConfigurationServiceDataFilename))
            {
                using (var configFile = _storage.OpenFile(StaticConfiguration.ConfigurationServiceDataFilename, FileMode.Open, FileAccess.Read))
                    _configuration = (ClientConfig)_serializer.ReadObject(configFile);
            }

            if (_configuration == null ||
                _configuration.LastUpdate.AddSeconds(_configuration.UpdateTimeout) > DateTimeOffset.UtcNow)
            {
                var req = _requestFactory.CreateRequest(new Uri(_staticConfiguration.ClientConfigUri));

#if (WP80)
                var res = (HttpWebResponse) await Task.Factory.FromAsync(req.BeginGetResponse, ar => req.EndGetResponse(ar), null);
#endif

#if (NET45)
                var res = (HttpWebResponse)await req.GetResponseAsync();
#endif
                //if (res.StatusCode != HttpStatusCode.OK)
                //    return false;

                _configuration = (ClientConfig)_serializer.ReadObject(res.GetResponseStream());
                _configuration.LastUpdate = DateTimeOffset.UtcNow;

                using (var configFile = _storage.OpenFile(StaticConfiguration.ConfigurationServiceDataFilename, FileMode.Create, FileAccess.Write))
                    _serializer.WriteObject(configFile, _configuration);
            }
        }
    }
}
