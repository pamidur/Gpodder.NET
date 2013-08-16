using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GpodderLib.RemoteServices.Devices.Dto
{
    [CollectionDataContract]
    class DeviceSet : List<Device>
    {
    }
}
