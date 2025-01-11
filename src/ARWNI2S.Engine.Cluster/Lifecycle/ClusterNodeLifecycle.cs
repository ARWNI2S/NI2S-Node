using ARWNI2S.Engine.Lifecycle;
using ARWNI2S.Lifecycle;
using Microsoft.Extensions.Logging;

namespace ARWNI2S.Cluster.Lifecycle
{
    /// <summary>
    /// Implementation of <see cref="IClusterNodeLifecycle"/>.
    /// </summary>
    internal class ClusterNodeLifecycle : LifecycleSubject, IClusterNodeLifecycle
    {
        private readonly IEngineLifecycleSubject _engineLifecycle;

        public ClusterNodeLifecycle(IEngineLifecycleSubject engineLifecycle, ILogger<ClusterNodeLifecycle> logger)
            : base(logger)
        {
            _engineLifecycle = engineLifecycle;
        }

        public override async Task OnStart(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("ClusterNodeLifecycle: Starting...");
            await base.OnStart(cancellationToken);
            await _engineLifecycle.OnStart(cancellationToken); // Delegación al engine lifecycle
        }

        public override async Task OnStop(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("ClusterNodeLifecycle: Stopping...");
            await _engineLifecycle.OnStop(cancellationToken); // Delegación al engine lifecycle
            await base.OnStop(cancellationToken);
        }
    }
}