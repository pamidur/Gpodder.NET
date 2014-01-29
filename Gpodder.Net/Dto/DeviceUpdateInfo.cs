using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GpodderLib.Dto
{
    [DataContract]
    public class DeviceUpdateInfo
    {
        public DeviceUpdateInfo()
        {
            AddedPodcasts = new List<Podcast>();
            //RemovedPodcasts = new List<Uri>();
            //UpdatedEpisodes = new List<string>();
        }

        [DataMember(Name = "add")]
        public List<Podcast> AddedPodcasts { get; set; }

        //[DataMember(Name = "rem")]
        //public List<Uri> RemovedPodcasts { get; set; }

        //[DataMember(Name = "updates")]
        //public List<string> UpdatedEpisodes { get; set; }

        //[DataMember(Name = "timestamp")]
        //public DateTimeOffset Timestamp { get; set; }
    }
}
