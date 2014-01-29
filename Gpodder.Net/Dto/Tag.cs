using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GpodderLib.Dto
{
    [DataContract]
    public class Tag
    {
        [DataMember(Name = "tag")]
        public string Name { get; set; }

        [DataMember(Name = "usage")]
        public int Usage { get; set; }
    }
}
