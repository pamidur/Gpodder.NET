using System.Runtime.Serialization;

namespace GpodderLib.Dto
{
    [DataContract]
    public class KnownDeviceInfo : DeviceInfo
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "subscriptions")]
        public int SubscriptionsCount { get; set; }
    }
}

