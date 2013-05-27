using System;
using System.Runtime.Serialization;

namespace GpodderLib.RemoteServices.Configuration.Dto
{
    [DataContract]
    class GpodderServiceConfig
    {
        [DataMember(Name = "baseurl")]
        public Uri BaseUrl { get; set; }
    }
}
