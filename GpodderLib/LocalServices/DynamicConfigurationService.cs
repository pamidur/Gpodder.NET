using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using GpodderLib.RemoteServices.Configuration;
using GpodderLib.RemoteServices.Configuration.Dto;

namespace GpodderLib.LocalServices
{
    [DataContract]
    internal class DynamicConfigurationService : ServiceBase
    {
        [DataMember]
        public DateTimeOffset LastServerSync { get; set; }

        [DataMember]
        public DateTimeOffset LastClientConfigSync { get; set; }

        [DataMember]
        public ClientConfig ClientConfigData { get; set; }

        [DataMember]
        public string DeviceId { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public Cookie SessionId { get; set; }

        public CookieContainer ClientSession { get; set; }

        public bool IsLoogedIn
        {
            get
            {
                var sessionCoockie = SessionId;

                if (sessionCoockie == null || sessionCoockie.Expired)
                    return false;

                return true;
            }
        }
        

        public Task SaveTo(Stream configurationData)
        {
            return Task.Run(() =>
            {
                var serializer = new DataContractJsonSerializer(typeof(DynamicConfigurationService));
                configurationData.Seek(0, SeekOrigin.Begin);
                serializer.WriteObject(configurationData, this);
                configurationData.SetLength(configurationData.Position);
            });
        }


        public static Task<DynamicConfigurationService> LoadFrom(Stream configurationData)
        {
            if (!configurationData.CanRead || !configurationData.CanWrite || !configurationData.CanSeek)
                throw new ArgumentException(
                    "Configuration data stream should be able to be read, written and sought over.");

            return Task.Run(() =>
            {
                var serializer = new DataContractJsonSerializer(typeof(DynamicConfigurationService));
                configurationData.Seek(0, SeekOrigin.Begin);

                try
                {
                    return (DynamicConfigurationService)serializer.ReadObject(configurationData);
                }
                catch
                {
                    return new DynamicConfigurationService();
                }

            });
        }
    }
}
