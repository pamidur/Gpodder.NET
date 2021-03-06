﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GpodderLib.Dto
{
    [CollectionDataContract]
    public class SuggestionSet : List<Suggestion>
    {
    }

    [DataContract]
    public class Suggestion
    {
        [DataMember(Name = "website")]
        public Uri Website { get; set; }

        [DataMember(Name = "mygpo_link")]
        public Uri GpodderLink { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "subscribers")]
        public int SubscribersCount { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "url")]
        public Uri FeedUrl { get; set; }

        [DataMember(Name = "subscribers_last_week")]
        public int SubscribersLastWeekCount { get; set; }

        [DataMember(Name = "logo_url")]
        public Uri LogoUrl { get; set; }
    }
}
