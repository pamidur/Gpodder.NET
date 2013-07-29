using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using GpodderLib.RemoteServices.Configuration;
using GpodderLib.RemoteServices.Configuration.Dto;

namespace GpodderLib
{
    [DataContract]
    class DynamicConfiguration
    {
        public DynamicConfiguration()
        {
            ClientSession = new CookieContainer();
        }

        [DataMember]
        public DateTimeOffset LastServerSync { get; set; }

        [DataMember]
        public DateTimeOffset LastClientConfigSync { get; set; }

        [DataMember]
        public ClientConfig ClientConfigData { get; set; }

        public string DeviceId { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Password { get; set; }

        public bool IsClientAuthenticated { get; set; }

        [DataMember]
        public CookieContainer ClientSession { get; set; }
    }
}
