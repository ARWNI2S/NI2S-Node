﻿using ARWNI2S.Infrastructure;
using ARWNI2S.Node;

namespace ARWNI2S.Entities.Clustering
{
    /// <summary>
    /// Represents a server.
    /// </summary>
    public interface INiisNode : IEntity, IDisposable
    {
        /// <summary>
        /// A collection of HTTP features of the server.
        /// </summary>
        IFeatureCollection Features { get; }

        /// <summary>
        /// Start the server with an application.
        /// </summary>
        /// <param name="application">An instance of <see cref="INiisEngine{TContext}"/>.</param>
        /// <typeparam name="TContext">The context associated with the application.</typeparam>
        /// <param name="cancellationToken">Indicates if the server startup should be aborted.</param>
        Task StartAsync<TContext>(INiisEngine<TContext> application, CancellationToken cancellationToken) where TContext : notnull;

        /// <summary>
        /// Stop processing requests and shut down the server, gracefully if possible.
        /// </summary>
        /// <param name="cancellationToken">Indicates if the graceful shutdown should be aborted.</param>
        Task StopAsync(CancellationToken cancellationToken);
    }
}
