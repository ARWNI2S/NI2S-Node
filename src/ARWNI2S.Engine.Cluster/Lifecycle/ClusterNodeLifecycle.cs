using ARWNI2S.Lifecycle;
using Microsoft.Extensions.Logging;

namespace ARWNI2S.Engine.Cluster.Lifecycle
{
    /// <summary>
    /// Implementation of <see cref="IClusterNodeLifecycle"/>.
    /// </summary>
    internal class ClusterNodeLifecycle : LifecycleSubject, IClusterNodeLifecycle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClusterNodeLifecycle"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ClusterNodeLifecycle(ILogger logger) : base(logger)
        {
        }
    }
}