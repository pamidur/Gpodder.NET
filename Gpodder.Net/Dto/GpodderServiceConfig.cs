using System;
using System.Runtime.Serialization;

namespace GpodderLib.Dto
{
    [DataContract]
    public class GpodderServiceConfig
    {
        [DataMember(Name = "baseurl")]
        public Uri BaseUrl { get; set; }
    }
}
