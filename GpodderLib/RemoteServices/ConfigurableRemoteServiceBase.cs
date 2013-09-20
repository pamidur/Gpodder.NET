using GpodderLib.LocalServices;
using GpodderLib.RemoteServices.Configuration;

namespace GpodderLib.RemoteServices
{
    public abstract class ConfigurableRemoteServiceBase : RemoteServiceBase
    {
        protected ConfigurationService ConfigurationService { get; private set; }

        protected ConfigurableRemoteServiceBase(
            StaticConfiguration staticConfiguration, 
            DynamicConfiguration dynamicConfiguration, 
            ConfigurationService configurationService)
            : base(staticConfiguration, dynamicConfiguration)
        {
            ConfigurationService = configurationService;
        }
    }
}
