using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GpodderLib.Services.Base
{
    public abstract class SecuredRemoteServiceBase : ConfigurableRemoteServiceBase
    {
        protected AuthenticationService AuthenticationService { get; private set; }

        protected SecuredRemoteServiceBase(
            Configuration configuration,
            ConfigurationService configurationService,
            AuthenticationService authenticationService)
            : base(configuration, configurationService)
        {
            AuthenticationService = authenticationService;
        }



        protected override async Task<HttpRequestMessage> CreateRequest(System.Uri uri, object argument = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var getRequestJob = base.CreateRequest(uri, argument, cancellationToken);
            var loginJob = AuthenticationService.Login(cancellationToken);

            await Task.WhenAll(
                getRequestJob,
                loginJob
                ).ConfigureAwait(false);

            var request = getRequestJob.Result;

            request.Headers.Add("Cookie",AuthenticationService.SessionId.ToString());

            return request;
        }
    }
}
