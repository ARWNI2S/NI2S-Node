using ARWNI2S.Node.Features;

namespace ARWNI2S.Node.Hosting
{
    /// <summary>
    /// Represents a configured web host.
    /// </summary>
    public interface INI2SHost : IDisposable
    {
        /// <summary>
        /// The <see cref="IFeatureCollection"/> exposed by the configured server.
        /// </summary>
        IFeatureCollection NodeFeatures { get; }

        /// <summary>
        /// The <see cref="IServiceProvider"/> for the host.
        /// </summary>
        IServiceProvider Services { get; }

        /// <summary>
        /// Starts listening on the configured addresses.
        /// </summary>
        void Start();

        /// <summary>
        /// Starts listening on the configured addresses.
        /// </summary>
        /// <param name="cancellationToken">Used to abort program start.</param>
        /// <returns>A <see cref="Task"/> that completes when the <see cref="INI2SHost"/> starts.</returns>
        Task StartAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Attempt to gracefully stop the host.
        /// </summary>
        /// <param name="cancellationToken">Used to indicate when stop should no longer be graceful.</param>
        /// <returns>A <see cref="Task"/> that completes when the <see cref="INI2SHost"/> stops.</returns>
        Task StopAsync(CancellationToken cancellationToken = default);
    }
}
