using System;
using System.Threading;
using System.Threading.Tasks;
using GpodderLib.Dto;
using GpodderLib.Services.Base;

namespace GpodderLib.Services
{
    public class ConfigurationService : RemoteServiceBase
    {
        private static readonly string _clientConfigUri = "https://gpodder.net/clientconfig.json";

        private Task<ClientConfig> _getConfigTask;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly Task _updateLoop;


        public ConfigurationService(Configuration configuration) : base(configuration)
        {
            _updateLoop = UpdateConfigLoop(_cancellationTokenSource.Token);
        }

        public async Task<ClientConfig> GetClientConfig()
        {
            return await _getConfigTask;
        }

        private async Task UpdateConfigLoop(CancellationToken cancellationToken)
        {
            _getConfigTask = QueryClientConfig(cancellationToken);
            await _getConfigTask;
            
            var msToWaitBeforeUpdate = Configuration.ClientConfigData == null
                                           ? 0
                                           : Convert.ToInt32(
                                               (Configuration.ClientConfigData.UpdateTimeout -
                                                (DateTimeOffset.UtcNow -
                                                 Configuration.LastClientConfigSync)
                                                    .TotalSeconds))*1000;

            await Task.Delay(msToWaitBeforeUpdate, cancellationToken);

            if (!cancellationToken.IsCancellationRequested)
                await UpdateConfigLoop(cancellationToken);
        }

        private async Task<ClientConfig> QueryClientConfig(CancellationToken cancellationToken)
        {
            if (Configuration.ClientConfigData == null ||
                Configuration.LastClientConfigSync.AddSeconds(
                    Configuration.ClientConfigData.UpdateTimeout) < DateTimeOffset.UtcNow)
            {
                Configuration.ClientConfigData =
                    await Query<ClientConfig>(new Uri(_clientConfigUri));
                Configuration.LastClientConfigSync = DateTimeOffset.Now;
            }

            return Configuration.ClientConfigData;
        }

        ~ConfigurationService()
        {
            try
            {
                _cancellationTokenSource.Cancel();
                _updateLoop.Wait();
            }
            catch (AggregateException e)
            {
                e.Handle(ex => ex is TaskCanceledException);
            }
        }
    }
}
