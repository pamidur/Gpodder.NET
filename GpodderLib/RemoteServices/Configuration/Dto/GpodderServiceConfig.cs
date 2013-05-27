using System;
using System.Runtime.Serialization;

namespace GpodderLib.RemoteServices.Configuration.Dto
{
    [DataContract]
    public class GpodderServiceConfig
    {
        [DataMember(Name = "baseurl")]
        public Uri BaseUrl { get; set; }
    }
}
