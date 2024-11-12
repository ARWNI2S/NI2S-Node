using Microsoft.Extensions.Logging;

namespace ARWNI2S.Infrastructure.Lifecycle
{
    /// <summary>
    /// Implementation of <see cref="IEngineLifecycle"/>.
    /// </summary>
    internal class EngineLifecycle : LifecycleSubject, IEngineLifecycle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EngineLifecycle"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public EngineLifecycle(ILogger logger) : base(logger)
        {
        }
    }
}