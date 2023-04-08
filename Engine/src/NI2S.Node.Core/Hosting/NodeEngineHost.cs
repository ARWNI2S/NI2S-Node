// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.Hosting;
using NI2S.Engine;
using NI2S.Node.Hosting.Builder;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NI2S.Node.Hosting
{
    /// <summary>
    /// Used to configure the NI2S node engine host.
    /// </summary>
    public sealed class NodeEngineHost : IHost, IEngineBuilder, IClusterNodeBuilder, IAsyncDisposable
    {
        private readonly IHost _host;

        internal NodeEngineHost(IHost host)
        {
            _host = host;
        }

        #region IEngineBuilder implementation

        public IServiceProvider EngineServices { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IModuleCollection EngineModules => throw new NotImplementedException();

        public IDictionary<object, object> Properties => throw new NotImplementedException();

        IEngineBuilder IEngineBuilder.New()
        {
            throw new NotImplementedException();
        }

        INodeEngine IEngineBuilder.Build()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IHost implementation

        public IServiceProvider Services => _host.Services;

        /// <summary>
        /// Start the NI2S node engine host.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to propagate cancellation notifications.</param>
        /// <returns>
        /// A <see cref="Task"/> that represents the startup of the <see cref="NodeEngineHost"/>.
        /// Successful completion indicates the NI2S engine is ready up and running.
        /// </returns>
        public Task StartAsync(CancellationToken cancellationToken = default) =>
            _host.StartAsync(cancellationToken);

        /// <summary>
        /// Shuts down the NI2S node engine host.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to propagate cancellation notifications.</param>
        /// <returns>
        /// A <see cref="Task"/> that represents the shutdown of the <see cref="NodeEngineHost"/>.
        /// Successful completion indicates that all the NI2S engine has stopped.
        /// </returns>
        public Task StopAsync(CancellationToken cancellationToken = default) =>
            _host.StopAsync(cancellationToken);

        #endregion

        /// <summary>
        /// Runs the node engine host and blocks the calling thread until host shutdown.
        /// </summary>
        public void Run()
        {
            //Listen(url);
            HostingAbstractionsHostExtensions.Run(this);
        }

        /// <summary>
        /// Runs the node engine host and returns a <see cref="Task"/> that only completes when the token is triggered or shutdown is triggered.
        /// The <see cref="IHost"/> instance is disposed of after running.
        /// </summary>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public Task RunAsync()
        {
            //Listen(url);
            return HostingAbstractionsHostExtensions.RunAsync(this);
        }

        #region Static class members

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeEngineHost"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>The <see cref="NodeEngineHost"/>.</returns>
        public static NodeEngineHost Create(string[] args = null) =>
            new NodeEngineHostBuilder(args).Build();

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeEngineHostBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <returns>The <see cref="NodeEngineHostBuilder"/>.</returns>
        public static NodeEngineHostBuilder CreateBuilder() =>
            new(null);

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeEngineHostBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>The <see cref="NodeEngineHostBuilder"/>.</returns>
        public static NodeEngineHostBuilder CreateBuilder(string[] args) =>
            new(args);

        #endregion

        #region Disposable implementations

        /// <summary>
        /// Disposes the NI2S node engine host.
        /// </summary>
        void IDisposable.Dispose() => _host.Dispose();

        /// <summary>
        /// Disposes the NI2S node engine host.
        /// </summary>
        public ValueTask DisposeAsync() => ((IAsyncDisposable)_host).DisposeAsync();

        #endregion
    }
}
