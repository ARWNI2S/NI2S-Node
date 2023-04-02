﻿using Microsoft.AspNetCore.Http.Features;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NI2S.Node.Hosting.Server
{
    /// <summary>
    /// Represents a server.
    /// </summary>
    public interface IServer : IDisposable
    {
        /// <summary>
        /// A collection of HTTP features of the server.
        /// </summary>
        IFeatureCollection Features { get; }

        /// <summary>
        /// Start the server with an application.
        /// </summary>
        /// <param name="application">An instance of <see cref="IDummyApplication{TContext}"/>.</param>
        /// <typeparam name="TContext">The context associated with the application.</typeparam>
        /// <param name="cancellationToken">Indicates if the server startup should be aborted.</param>
        Task StartAsync<TContext>(IDummyApplication<TContext> application, CancellationToken cancellationToken) where TContext : notnull;

        /// <summary>
        /// Stop processing requests and shut down the server, gracefully if possible.
        /// </summary>
        /// <param name="cancellationToken">Indicates if the graceful shutdown should be aborted.</param>
        Task StopAsync(CancellationToken cancellationToken);
    }
}
