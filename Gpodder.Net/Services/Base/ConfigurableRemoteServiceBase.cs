namespace GpodderLib.Services.Base
{
    public abstract class ConfigurableRemoteServiceBase : RemoteServiceBase
    {
        protected ConfigurationService ConfigurationService { get; private set; }

        protected ConfigurableRemoteServiceBase( 
            Configuration configuration, 
            ConfigurationService configurationService)
            : base(configuration)
        {
            ConfigurationService = configurationService;
        }
    }
}
