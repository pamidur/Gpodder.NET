using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GpodderLib.Dto
{
    [DataContract]
    public class Podcast
    {
        [DataMember(Name = "website")]
        public Uri Website { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "url")]
        public Uri Url { get; set; }

        [DataMember(Name = "position_last_week")]
        public int PositionLastWeek { get; set; }

        [DataMember(Name = "subscribers_last_week")]
        public int SubscribersLastWeek { get; set; }

        [DataMember(Name = "subscribers")]
        public int Subscribers { get; set; }

        [DataMember(Name = "mygpo_link")]
        public Uri GpodderLink { get; set; }

        [DataMember(Name = "logo_url")]
        public Uri LogoUrl { get; set; }

        [DataMember(Name = "scaled_logo_url")]
        public Uri ScaledLogoUrl { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}
