using System;
using System.Runtime.Serialization;

namespace GpodderLib.RemoteServices.Configuration.Dto
{
    [DataContract]
    class ClientConfig
    {
        [DataMember(Name = "mygpo")]
        public GpodderServiceConfig ApiConfig { get; set; }

        [DataMember(Name = "mygpo-feedservice")]
        public GpodderServiceConfig FeedServiceConfig { get; set; }

        [DataMember(Name = "update_timeout")]
        public int UpdateTimeout { get; set; }
    }
}

