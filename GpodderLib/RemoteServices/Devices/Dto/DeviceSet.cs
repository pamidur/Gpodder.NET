using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GpodderLib.RemoteServices.Devices.Dto
{
    [CollectionDataContract]
    public class DeviceSet : List<Device>
    {
    }
}
