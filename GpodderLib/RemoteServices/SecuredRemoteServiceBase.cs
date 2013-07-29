using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GpodderLib.RemoteServices.Authentication;

namespace GpodderLib.RemoteServices
{
    abstract class SecuredRemoteServiceBase:RemoteServiceBase
    {
        protected AuthenticationService AuthenticationService { get; set; }

        protected SecuredRemoteServiceBase()
        {
            AuthenticationService = ServiceLocator.Instance.GetService<AuthenticationService>();
        }

        protected override async Task<TR> Query<TA, TR>(Uri uri, TA argument)
        {
            if (!DynamicConfiguration.IsClientAuthenticated)
            {
                await AuthenticationService.Login(DynamicConfiguration.Username, DynamicConfiguration.Password);
                DynamicConfiguration.IsClientAuthenticated = true;
            }

            return await base.Query<TA, TR>(uri, argument);
        }
    }
}
