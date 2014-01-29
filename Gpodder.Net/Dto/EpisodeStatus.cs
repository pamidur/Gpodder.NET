using System.Runtime.Serialization;

namespace GpodderLib.Dto
{
    [DataContract]
    public enum EpisodeStatus
    {
        [EnumMember(Value = "new")]
        New,
        [EnumMember(Value = "play")]
        Play,
        [EnumMember(Value = "download")]
        Download,
        [EnumMember(Value = "delete")]
        Delete
    }
}
