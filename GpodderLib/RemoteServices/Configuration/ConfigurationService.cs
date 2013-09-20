using System;
using System.Threading;
using System.Threading.Tasks;
using GpodderLib.LocalServices;
using GpodderLib.RemoteServices.Configuration.Dto;

namespace GpodderLib.RemoteServices.Configuration
{
    public class ConfigurationService : RemoteServiceBase
    {
        private Task<ClientConfig> _getConfigTask;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly Task _updateLoop;


        public ConfigurationService(StaticConfiguration staticConfiguration, DynamicConfiguration dynamicConfiguration) : base(staticConfiguration, dynamicConfiguration)
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
            
            var msToWaitBeforeUpdate = DynamicConfiguration.ClientConfigData == null
                                           ? 0
                                           : Convert.ToInt32(
                                               (DynamicConfiguration.ClientConfigData.UpdateTimeout -
                                                (DateTimeOffset.UtcNow -
                                                 DynamicConfiguration.LastClientConfigSync)
                                                    .TotalSeconds))*1000;

            await Task.Delay(msToWaitBeforeUpdate, cancellationToken);

            if (!cancellationToken.IsCancellationRequested)
                await UpdateConfigLoop(cancellationToken);
        }

        private async Task<ClientConfig> QueryClientConfig(CancellationToken cancellationToken)
        {
            if (DynamicConfiguration.ClientConfigData == null ||
                DynamicConfiguration.LastClientConfigSync.AddSeconds(
                    DynamicConfiguration.ClientConfigData.UpdateTimeout) < DateTimeOffset.UtcNow)
            {
                DynamicConfiguration.ClientConfigData =
                    await Query<ClientConfig>(new Uri(StaticConfiguration.ClientConfigUri));
                DynamicConfiguration.LastClientConfigSync = DateTimeOffset.Now;
            }

            return DynamicConfiguration.ClientConfigData;
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
