using NI2S.Node.Engine.Modules;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NI2S.Node.Dummy
{
    /// <summary>
    /// Represents a server.
    /// </summary>
    public interface IDummyServer : IDisposable
    {
        /// <summary>
        /// A collection of HTTP features of the server.
        /// </summary>
        IDummyFeatureCollection Features { get; }

        /// <summary>
        /// Start the server with an node.
        /// </summary>
        /// <param name="node">An instance of <see cref="IDummyApplication{TContext}"/>.</param>
        /// <typeparam name="TContext">The context associated with the node.</typeparam>
        /// <param name="cancellationToken">Indicates if the server startup should be aborted.</param>
        Task StartAsync<TContext>(IDummyApplication<TContext> node, CancellationToken cancellationToken) where TContext : notnull;

        /// <summary>
        /// Stop processing requests and shut down the server, gracefully if possible.
        /// </summary>
        /// <param name="cancellationToken">Indicates if the graceful shutdown should be aborted.</param>
        Task StopAsync(CancellationToken cancellationToken);
    }
}
