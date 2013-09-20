using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GpodderLib.RemoteServices.Suggestions.Dto
{
    [CollectionDataContract]
    public class SuggestionSet : List<SuggestionItem>
    {
    }
}
