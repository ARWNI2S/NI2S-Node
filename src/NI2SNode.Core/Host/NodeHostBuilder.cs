using NI2S.Node.Engine.Modules;
using NI2S.Node.Infrastructure;

namespace NI2S.Node.Host
{
    /// <summary>
    /// Default implementation for <see cref="INodeHostBuilder"/>.
    /// </summary>
    public class NodeHostBuilder : INodeHostBuilder
    {
        private const string EngineModulesKey = "engine.Modules";
        private const string NodeServicesKey = "node.Services";

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> for application services.
        /// </summary>
        public IServiceProvider NodeServices
        {
            get
            {
                return GetProperty<IServiceProvider>(NodeServicesKey)!;
            }
            set
            {
                SetProperty<IServiceProvider>(NodeServicesKey, value);
            }
        }

        /// <summary>
        /// Gets the <see cref="IFeatureCollection"/> for server features.
        /// </summary>
        /// <remarks>
        /// An empty collection is returned if a server wasn't specified for the application builder.
        /// </remarks>
        public IModuleCollection EngineModules
        {
            get
            {
                return GetProperty<IModuleCollection>(EngineModulesKey)!;
            }
        }

        /// <summary>
        /// Gets a set of properties for <see cref="NodeHostBuilder"/>.
        /// </summary>
        public IDictionary<string, object?> Properties { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="NodeHostBuilder"/>.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> for node services.</param>
        public NodeHostBuilder(IServiceProvider serviceProvider) 
            : this(serviceProvider, new ModuleCollection()) { }

        /// <summary>
        /// Initializes a new instance of <see cref="NodeHostBuilder"/>.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> for node services.</param>
        /// <param name="engine">The engine instance that hosts the node.</param>
        public NodeHostBuilder(IServiceProvider serviceProvider, object engine)
        {
            Properties = new Dictionary<string, object?>(StringComparer.Ordinal);
            NodeServices = serviceProvider;

            SetProperty(EngineModulesKey, engine);
        }

        private NodeHostBuilder(NodeHostBuilder builder)
        {
            Properties = new CopyOnWriteDictionary<string, object?>(builder.Properties, StringComparer.Ordinal);
        }

        private T? GetProperty<T>(string key)
        {
            return Properties.TryGetValue(key, out var value) ? (T?)value : default;
        }

        private void SetProperty<T>(string key, T value)
        {
            Properties[key] = value;
        }

        private INodeHostBuilder New()
        {
            return new NodeHostBuilder(this);
        }

        INodeHostBuilder INodeHostBuilder.CreateNodeBuilder() => New();
    }
}
