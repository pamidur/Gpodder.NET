using System.ComponentModel;
using System.Runtime.Serialization;
using GpodderLib.Utils;

namespace GpodderLib.Dto
{
    [DataContract]
    public class DeviceInfo
    {
        [DataMember(Name = "caption")]
        public string Caption { get; set; }

        [DataMember(Name = "type")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string TypeRaw
        {
            get { return Type.GetValueName(); }
            set { Type = value.GetEnumValue<DeviceType>(); }
        }
        
        public DeviceType Type { get; set; }
    }
}