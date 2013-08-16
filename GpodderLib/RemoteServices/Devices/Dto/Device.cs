using System;
using System.Runtime.Serialization;

namespace GpodderLib.RemoteServices.Devices.Dto
{
    [DataContract]
    public class Device
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "caption")]
        public string Caption { get; set; }

        [DataMember(Name = "type")]
        public string TypeRaw
        {
            get { return Enum.GetName(typeof(DeviceType), Type); }
            set { Type = (DeviceType)Enum.Parse(typeof(DeviceType), value, true); }
        }

        [DataMember(Name = "subscriptions")]
        public int SubscriptionsCount { get; set; }

        public DeviceType Type { get; set; }
    }
}

