using System;
using System.Threading.Tasks;
using GpodderLib.RemoteServices.Authentication;

namespace GpodderLib.RemoteServices
{
    abstract class SecuredRemoteServiceBase:RemoteServiceBase
    {
        protected AuthenticationService AuthenticationService { get; set; }

        public override async Task Init()
        {
            await base.Init();
            AuthenticationService = ServiceLocator.Get<AuthenticationService>();
        }

        protected override async Task<TR> Query<TA, TR>(Uri uri, TA argument)
        {
            if (!DynamicConfigurationService.IsLoogedIn)
            {
                await AuthenticationService.Login();
            }

            return await base.Query<TA, TR>(uri, argument);
        }
    }
}
