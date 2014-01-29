using System.Runtime.Serialization;

namespace GpodderLib.Dto
{
    [DataContract]
    public enum DeviceType
    {
        [EnumMember(Value = "desktop")]
        Desktop,
        [EnumMember(Value = "laptop")]
        Laptop,
        [EnumMember(Value = "mobile")]
        Mobile,
        [EnumMember(Value = "server")]
        Server,
        [EnumMember(Value = "other")]
        Other
    }
}
