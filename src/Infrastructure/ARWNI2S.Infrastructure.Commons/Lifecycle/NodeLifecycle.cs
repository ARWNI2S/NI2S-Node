using Microsoft.Extensions.Logging;

namespace ARWNI2S.Infrastructure.Lifecycle
{
    /// <summary>
    /// Implementation of <see cref="INodeLifecycle"/>.
    /// </summary>
    internal class NodeLifecycle : LifecycleSubject, INodeLifecycle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeLifecycle"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public NodeLifecycle(ILogger logger) : base(logger)
        {
        }
    }
}