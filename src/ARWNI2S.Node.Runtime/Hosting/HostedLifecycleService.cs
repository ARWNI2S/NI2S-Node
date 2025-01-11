// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;

namespace ARWNI2S.Node.Hosting
{
    internal abstract class HostedLifecycleService : IHostedLifecycleService
    {
        private readonly ILogger<HostedLifecycleService> _logger;
        //private readonly SynchronizationContext _syncContext;

        public ILifecycleSubject Lifecycle { get; }


        public HostedLifecycleService(ILoggerFactory logger, ILifecycleSubject lifecycle)
        {
            _logger = logger.CreateLogger<HostedLifecycleService>();
            Lifecycle = lifecycle;

            //_syncContext = SynchronizationContext.Current ?? new SynchronizationContext();
        }

        public virtual Task StartingAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{name}: {method} - Thread: {thread}", nameof(HostedLifecycleService), nameof(IHostedLifecycleService.StartingAsync), System.Environment.CurrentManagedThreadId);
            return Task.CompletedTask;
        }

        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{name}: {method} - Thread: {thread}", nameof(HostedLifecycleService), nameof(IHostedLifecycleService.StartAsync), System.Environment.CurrentManagedThreadId);

            try
            {
                //await Lifecycle.OnStart(cancellationToken);
                await Lifecycle.OnStart(cancellationToken).ConfigureAwait(true);
                _logger.LogInformation("Engine lifecycle started. - Thread: {thread}", System.Environment.CurrentManagedThreadId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting lifecycles.");
                throw;
            }
        }

        public virtual Task StartedAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{name}: {method} - Thread: {thread}", nameof(HostedLifecycleService), nameof(IHostedLifecycleService.StartedAsync), System.Environment.CurrentManagedThreadId);
            return Task.CompletedTask;
        }


        public virtual Task StoppingAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{name}: {method} - Thread: {thread}", nameof(HostedLifecycleService), nameof(IHostedLifecycleService.StoppingAsync), System.Environment.CurrentManagedThreadId);
            return Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{name}: {method} - Thread: {thread}", nameof(HostedLifecycleService), nameof(IHostedLifecycleService.StopAsync), System.Environment.CurrentManagedThreadId);

            try
            {
                //await Lifecycle.OnStop(cancellationToken);
                await Lifecycle.OnStop(cancellationToken).ConfigureAwait(true);
                _logger.LogInformation("Engine lifecycle stopped. - Thread: {thread}", System.Environment.CurrentManagedThreadId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error stopping lifecycles.");
                throw;
            }
        }

        public virtual Task StoppedAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{name}: {method} - Thread: {thread}", nameof(HostedLifecycleService), nameof(IHostedLifecycleService.StoppedAsync), System.Environment.CurrentManagedThreadId);
            return Task.CompletedTask;
        }
    }
}
