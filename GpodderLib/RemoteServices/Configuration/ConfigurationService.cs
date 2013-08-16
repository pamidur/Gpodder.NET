using System;
using System.Threading;
using System.Threading.Tasks;
using GpodderLib.RemoteServices.Configuration.Dto;

namespace GpodderLib.RemoteServices.Configuration
{
    class ConfigurationService : RemoteServiceBase
    {
        private Task<ClientConfig> _getConfigTask;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEventSlim _initEvent = new ManualResetEventSlim();

        public ConfigurationService()
        {
            UpdateConfigLoop(_cancellationTokenSource.Token);
        }

        public override async Task Init()
        {
            await base.Init();
            _initEvent.Set();
        }

        public async Task<ClientConfig> GetClientConfig()
        {
            return await _getConfigTask;
        }

        private async Task UpdateConfigLoop(CancellationToken cancellationToken)
        {
            _getConfigTask = QueryClientConfig(cancellationToken);
            await _getConfigTask;
            
            var msToWaitBeforeUpdate = DynamicConfigurationService.ClientConfigData == null
                                           ? 0
                                           : Convert.ToInt32(
                                               (DynamicConfigurationService.ClientConfigData.UpdateTimeout -
                                                (DateTimeOffset.UtcNow -
                                                 DynamicConfigurationService.LastClientConfigSync)
                                                    .TotalSeconds))*1000;

            await Task.Delay(msToWaitBeforeUpdate, cancellationToken);

            if (!cancellationToken.IsCancellationRequested)
                await UpdateConfigLoop(cancellationToken);
        }

        private async Task<ClientConfig> QueryClientConfig(CancellationToken cancellationToken)
        {
            await Task.Run(() => _initEvent.Wait(cancellationToken), cancellationToken);

            if (DynamicConfigurationService.ClientConfigData == null ||
                DynamicConfigurationService.LastClientConfigSync.AddSeconds(
                    DynamicConfigurationService.ClientConfigData.UpdateTimeout) < DateTimeOffset.UtcNow)
            {
                DynamicConfigurationService.ClientConfigData =
                    await Query<ClientConfig>(new Uri(StaticConfigurationService.ClientConfigUri));
                DynamicConfigurationService.LastClientConfigSync = DateTimeOffset.Now;

            }

            return DynamicConfigurationService.ClientConfigData;
        }

        ~ConfigurationService()
        {
            try
            {
                _cancellationTokenSource.Cancel();
            }
            catch (AggregateException e)
            {
                e.Handle(ex => ex is TaskCanceledException);
            }
        }
    }
}
