using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using GpodderLib.Utils;

namespace GpodderLib.Dto
{
    [DataContract]
    public class Episode
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "url")]
        public Uri Url { get; set; }

        [DataMember(Name = "podcast_title")]
        public string PodcastTitle { get; set; }

        [DataMember(Name = "podcast_url")]
        public Uri PodcastUrl { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "website")]
        public Uri Website { get; set; }

        [DataMember(Name = "mygpo_link")]
        public Uri GpodderLink { get; set; }

        [DataMember(Name = "released")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string ReleasedRaw
        {
            get { return Released.ToString("s"); }
            set { Released = DateTimeOffset.Parse(value); }
        }
        public DateTimeOffset Released{ get; set; }

        [DataMember(Name = "status")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string StatusRaw
        {
            get { return Status.GetValueName(); }
            set { Status = value.GetEnumValue<EpisodeStatus>(); }
        }
        public EpisodeStatus Status { get; set; }

    }
}
