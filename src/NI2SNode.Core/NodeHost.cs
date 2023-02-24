using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NI2S.Node.Engine;
using NI2S.Node.Engine.Modules;
using NI2S.Node.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NI2S.Node
{
    /// <summary>
    /// The NI2S node host used to configure the engine, and clustering.
    /// </summary>
    public sealed class NodeHost : IHost, INodeHostBuilder, IAsyncDisposable
    {
        internal const string GlobalEngineBuilderKey = "__GlobalEngineBuilderKey";

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeHost"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>The <see cref="NodeHost"/>.</returns>
        public static NodeHost Create(string[]? args = null) =>
            new NodeHostBuilder(new() { Args = args }).Build();

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeHostBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <returns>The <see cref="NodeHostBuilder"/>.</returns>
        public static NodeHostBuilder CreateBuilder() =>
            new(new());

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeHostBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>The <see cref="NodeHostBuilder"/>.</returns>
        public static NodeHostBuilder CreateBuilder(string[] args) =>
            new(new() { Args = args });

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeHostBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="options">The <see cref="NodeHostOptions"/> to configure the <see cref="NodeHostBuilder"/>.</param>
        /// <returns>The <see cref="NodeHostBuilder"/>.</returns>
        public static NodeHostBuilder CreateBuilder(NodeHostOptions options) =>
            new(options);

        private readonly IHost _host;
        private bool disposedValue;

        /// <summary>
        /// The default logger for the node.
        /// </summary>
        public ILogger Logger { get; }

        internal IModuleCollection EngineModules => _host.Services.GetRequiredService<IEngine>().Modules;

        internal NodeHostBuilder NodeHostBuilder { get; }

        internal NodeHost(IHost host)
        {
            _host = host;
            NodeHostBuilder = new NodeHostBuilder(host.Services, EngineModules);
            Logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(NodeHost));

            Properties[GlobalEngineBuilderKey] = this;
        }

        // REVIEW: Should this be wrapping another type?
        private INodeHostBuilder CreateBuilder()
        {
            var newBuilder = new NodeHostBuilder(this);

            // Remove the route builder so branched pipelines have their own routing world
            newBuilder.Properties.Remove(GlobalEngineBuilderKey);
            return newBuilder;
        }

        #region IHost Implementation
        IServiceProvider IHost.Services => throw new NotImplementedException();

        Task IHost.StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IHost.StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region INodeHostBuilder Implementation
        INodeHostBuilder INodeHostBuilder.ConfigureNodeServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            throw new NotImplementedException();
        }
        INodeHostBuilder INodeHostBuilder.CreateNodeBuilder() => CreateBuilder();

        #region INodeHostBuilder.IHostBuilder Implementation
        IDictionary<object, object> IHostBuilder.Properties => throw new NotImplementedException();

        IModuleCollection INodeHostBuilder.EngineModules => EngineModules;

        IHost IHostBuilder.Build()
        {
            throw new NotImplementedException();
        }

        IHostBuilder IHostBuilder.ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            throw new NotImplementedException();
        }

        IHostBuilder IHostBuilder.ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configureDelegate)
        {
            throw new NotImplementedException();
        }

        IHostBuilder IHostBuilder.ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
        {
            throw new NotImplementedException();
        }

        IHostBuilder IHostBuilder.ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            throw new NotImplementedException();
        }

        IHostBuilder IHostBuilder.UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory)
        {
            throw new NotImplementedException();
        }

        IHostBuilder IHostBuilder.UseServiceProviderFactory<TContainerBuilder>(Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region INodeHostBuilder.IMinimalApiHostBuilder Implementation
        void IMinimalApiHostBuilder.ConfigureHostBuilder()
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion

        #region IDisposable Implementation
        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: eliminar el estado administrado (objetos administrados)
                }

                // TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
                // TODO: establecer los campos grandes como NULL
                disposedValue = true;
            }
        }

        // // TODO: reemplazar el finalizador solo si "Dispose(bool disposing)" tiene código para liberar los recursos no administrados
        // ~NodeHost()
        // {
        //     // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
        //     Dispose(disposing: false);
        // }

        void IDisposable.Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region IAsyncDisposable Implementation
        ValueTask IAsyncDisposable.DisposeAsync()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
