using NI2S.Node.Engine.Modules;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NI2S.Node.Hosting
{
    /// <summary>
    /// Represents a configured node host.
    /// </summary>
    public interface INodeHost : IDisposable
    {
        /// <summary>
        /// The <see cref="IDummyFeatureCollection"/> exposed by the configured server.
        /// </summary>
        IDummyFeatureCollection ServerFeatures { get; }

        /// <summary>
        /// The <see cref="IServiceProvider"/> for the host.
        /// </summary>
        IServiceProvider Services { get; }

        /// <summary>
        /// Starts listening on the configured addresses.
        /// TODO: Node is not 'listening', but running and accepting connections, als connects to cluster etc...
        /// </summary>
        void Start();

        /// <summary>
        /// Starts listening on the configured addresses.
        /// TODO: Node is not 'listening', but running and accepting connections, als connects to cluster etc...
        /// </summary>
        /// <param name="cancellationToken">Used to abort program start.</param>
        /// <returns>A <see cref="Task"/> that completes when the <see cref="INodeHost"/> starts.</returns>
        Task StartAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Attempt to gracefully stop the host.
        /// </summary>
        /// <param name="cancellationToken">Used to indicate when stop should no longer be graceful.</param>
        /// <returns>A <see cref="Task"/> that completes when the <see cref="INodeHost"/> stops.</returns>
        Task StopAsync(CancellationToken cancellationToken = default);
    }
}
