using System;
//using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using GpodderLib.Dto;

namespace GpodderLib
{
    [DataContract]
    public class Configuration
    {
        //[DataMember]
        //public ConcurrentDictionary<string, object> Settings { get; set; }

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

        public Task SaveTo(Stream configurationData)
        {
            return Task.Run(() =>
            {
                var serializer = new DataContractJsonSerializer(typeof(Configuration));
                configurationData.Seek(0, SeekOrigin.Begin);
                serializer.WriteObject(configurationData, this);
                configurationData.SetLength(configurationData.Position);
            });
        }


        public static Task<Configuration> LoadFrom(Stream configurationData)
        {
            if (!configurationData.CanRead || !configurationData.CanWrite || !configurationData.CanSeek)
                throw new ArgumentException(
                    "Configuration data stream should be able to be read, written and sought over.");

            return Task.Run(() =>
            {
                var serializer = new DataContractJsonSerializer(typeof(Configuration));
                configurationData.Seek(0, SeekOrigin.Begin);

                try
                {
                    return (Configuration)serializer.ReadObject(configurationData);
                }
                catch
                {
                    return new Configuration();
                }

            });
        }
    }
}
