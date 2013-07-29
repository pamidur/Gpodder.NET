using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GpodderLib.RemoteServices.Suggestions.Dto
{
    [DataContract]
    internal class SuggestionSet : List<SuggestionItem>
    {
    }
}
