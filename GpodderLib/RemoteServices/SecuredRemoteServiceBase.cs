using System;
using System.Threading.Tasks;
using GpodderLib.LocalServices;
using GpodderLib.RemoteServices.Authentication;
using GpodderLib.RemoteServices.Configuration;

namespace GpodderLib.RemoteServices
{
    public abstract class SecuredRemoteServiceBase:ConfigurableRemoteServiceBase
    {
        protected AuthenticationService AuthenticationService { get; private set; }

        protected SecuredRemoteServiceBase(
            StaticConfiguration staticConfiguration,
            DynamicConfiguration dynamicConfiguration, 
            ConfigurationService configurationService,
            AuthenticationService authenticationService) 
            : base(staticConfiguration, dynamicConfiguration, configurationService)
        {
            AuthenticationService = authenticationService;
        }

        protected override async Task<TR> Query<TA, TR>(Uri uri, TA argument)
        {
            if (!DynamicConfiguration.IsLoogedIn)
            {
                await AuthenticationService.Login();
            }

            return await base.Query<TA, TR>(uri, argument);
        }
    }
}
