﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GpodderLib.RemoteServices.Suggestions.Dto
{
    [CollectionDataContract]
    internal class SuggestionSet : List<SuggestionItem>
    {
    }
}
