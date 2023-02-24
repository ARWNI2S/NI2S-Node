using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NI2S.Node.Engine.Modules;

namespace NI2S.Node.Host
{
    /// <summary>
    /// Defines a class that provides the mechanisms to configure an NI2S node engine.
    /// </summary>
    public interface INodeHostBuilder : IHostBuilder, IMinimalApiHostBuilder
    {
        /// <summary>
        /// Gets or sets the <see cref="IServiceProvider"/> that provides access to the node's service container.
        /// </summary>
        IServiceProvider NodeServices { get; set; }

        /// <summary>
        /// Gets the set of engine modules the node provides.
        /// </summary>
        /// <remarks>
        /// An empty collection is returned if a node was barebone deployed.
        /// </remarks>
        IModuleCollection EngineModules { get; }

        INodeHostBuilder ConfigureNodeServices(Action<HostBuilderContext, IServiceCollection> configureDelegate);

        INodeHostBuilder CreateNodeBuilder();
    }

    internal interface INodeHostBuilder<TReceivePackage>
    {
    }
}