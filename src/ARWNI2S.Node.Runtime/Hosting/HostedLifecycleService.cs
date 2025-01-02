using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;

namespace ARWNI2S.Node.Hosting
{
    internal abstract class HostedLifecycleService : IHostedLifecycleService
    {
        private readonly ILogger<HostedLifecycleService> _logger;

        public ILifecycleSubject Lifecycle { get; }

        //public IEngineLifecycleSubject EngineLifecycle { get; }
        //public IClusterNodeLifecycle NodeLifecycle { get; }

        public HostedLifecycleService(ILoggerFactory logger, ILifecycleSubject lifecycle)
        {
            _logger = logger.CreateLogger<HostedLifecycleService>();
            Lifecycle = lifecycle;
        }

        //public HostedLifecycleService(ILoggerFactory logger, IEngineLifecycleSubject engineLifecycle, IClusterNodeLifecycle nodeLifecycle)
        //{
        //    _logger = logger.CreateLogger<HostedLifecycleService>();
        //    EngineLifecycle = engineLifecycle;
        //    NodeLifecycle = nodeLifecycle;
        //}

        public virtual Task StartingAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{nameof(HostedLifecycleService)}: {nameof(IHostedLifecycleService.StartingAsync)} - Thread: {System.Environment.CurrentManagedThreadId}");
            return Task.CompletedTask;
        }

        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting Lifecycle Hosted Service");

            try
            {
                await Lifecycle.OnStart(cancellationToken);
                //await EngineLifecycle.OnStart(cancellationToken);
                _logger.LogInformation("Engine lifecycle started.");

                //await NodeLifecycle.OnStart(cancellationToken);
                //_logger.LogInformation("Node lifecycle started.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting lifecycles.");
                throw;
            }
        }

        public virtual Task StartedAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{nameof(HostedLifecycleService)}: {nameof(IHostedLifecycleService.StartedAsync)} - Thread: {System.Environment.CurrentManagedThreadId}");
            return Task.CompletedTask;
        }


        public virtual Task StoppingAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{nameof(HostedLifecycleService)}: {nameof(IHostedLifecycleService.StoppingAsync)} - Thread: {System.Environment.CurrentManagedThreadId}");
            return Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping Lifecycle Hosted Service");

            try
            {
                //await NodeLifecycle.OnStop(cancellationToken);
                //_logger.LogInformation("Node lifecycle stopped.");

                //await EngineLifecycle.OnStop(cancellationToken);
                await Lifecycle.OnStop(cancellationToken);
                _logger.LogInformation("Engine lifecycle stopped.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error stopping lifecycles.");
                throw;
            }
        }

        public virtual Task StoppedAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{nameof(HostedLifecycleService)}: {nameof(IHostedLifecycleService.StoppedAsync)} - Thread: {System.Environment.CurrentManagedThreadId}");
            return Task.CompletedTask;
        }
    }
}
