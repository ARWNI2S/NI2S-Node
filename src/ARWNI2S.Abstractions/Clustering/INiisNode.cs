using ARWNI2S.Engine;
using ARWNI2S.Entities;

namespace ARWNI2S.Clustering
{
    /// <summary>
    /// Represents a NI2S cluster node.
    /// </summary>
    public interface INiisNode : IEntity, IDisposable
    {
        /// <summary>
        /// A collection of features of the local node.
        /// </summary>
        IFeatureCollection Features { get; }

        /// <summary>
        /// Start the node with an application.
        /// </summary>
        /// <param name="application">An instance of <see cref="INiisEngine{TContext}"/>.</param>
        /// <typeparam name="TContext">The context associated with the application.</typeparam>
        /// <param name="cancellationToken">Indicates if the node startup should be aborted.</param>
        Task StartAsync<TContext>(INiisEngine<TContext> application, CancellationToken cancellationToken) where TContext : notnull;

        /// <summary>
        /// Stop processing requests and shut down the node, gracefully if possible.
        /// </summary>
        /// <param name="cancellationToken">Indicates if the graceful shutdown should be aborted.</param>
        Task StopAsync(CancellationToken cancellationToken);
    }
}
