using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace GpodderLib
{
    [DataContract]
    abstract class ServiceBase
    {
        [IgnoreDataMember]
        public ServiceLocator ServiceLocator { get; set; }

        public virtual async Task Init()
        {
            await Task.Run(() => { });
        }
    }
}
