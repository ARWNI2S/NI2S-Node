using ARWNI2S.Infrastructure.Engine.Builder;
using ARWNI2S.Node.Hosting.Builder;
using ARWNI2S.Node.Hosting.Configuration.Options;
using ARWNI2S.Node.Hosting.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace ARWNI2S.Node.Hosting.Internal
{
    /// <summary>
    /// A non-buildable <see cref="INodeHostBuilder"/> for NodeEngineBuilder/>.
    /// Use NodeEngineBuilder.Build to build the NodeEngineBuilder/>.
    /// </summary>
    public sealed class ConfigureNodeHostBuilder : INodeHostBuilder, ISupportsHostStartup
    {
        private readonly INodeHostEnvironment _environment;
        private readonly ConfigurationManager _configuration;
        private readonly IServiceCollection _services;
        private readonly NodeHostBuilderContext _context;

        internal ConfigureNodeHostBuilder(NodeHostBuilderContext nodeHostBuilderContext, ConfigurationManager configuration, IServiceCollection services)
        {
            _configuration = configuration;
            _environment = nodeHostBuilderContext.HostingEnvironment;
            _services = services;
            _context = nodeHostBuilderContext;
        }

        INodeHost INodeHostBuilder.Build()
        {
            throw new NotSupportedException($"Call NodeEngineBuilder.Build() instead.");
        }

        /// <inheritdoc />
        public INodeHostBuilder ConfigureAppConfiguration(Action<NodeHostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            var previousContentRoot = HostingPathResolver.ResolvePath(_context.HostingEnvironment.ContentRootPath);
            var previousContentRootConfig = _configuration[NodeHostDefaults.ContentRootKey];
            var previousNodeRoot = HostingPathResolver.ResolvePath(_context.HostingEnvironment.NodeRootPath, previousContentRoot);
            var previousNodeRootConfig = _configuration[NodeHostDefaults.NodeRootKey];
            var previousEngine = _configuration[NodeHostDefaults.EngineKey];
            var previousEnvironment = _configuration[NodeHostDefaults.EnvironmentKey];
            var previousHostingStartupAssemblies = _configuration[NodeHostDefaults.HostingStartupAssembliesKey];
            var previousHostingStartupAssembliesExclude = _configuration[NodeHostDefaults.HostingStartupExcludeAssembliesKey];

            // Run these immediately so that they are observable by the imperative code
            configureDelegate(_context, _configuration);

            if (!string.Equals(previousNodeRootConfig, _configuration[NodeHostDefaults.NodeRootKey], StringComparison.OrdinalIgnoreCase)
                && !string.Equals(HostingPathResolver.ResolvePath(previousNodeRoot, previousContentRoot), HostingPathResolver.ResolvePath(_configuration[NodeHostDefaults.NodeRootKey], previousContentRoot), StringComparison.OrdinalIgnoreCase))
            {
                // Diasllow changing the node root for consistency with other types.
                throw new NotSupportedException($"The node root changed from \"{HostingPathResolver.ResolvePath(previousNodeRoot, previousContentRoot)}\" to \"{HostingPathResolver.ResolvePath(_configuration[NodeHostDefaults.NodeRootKey], previousContentRoot)}\". Changing the host configuration using NodeEngineBuilder.NodeHost is not supported. Use NodeEngine.CreateBuilder(NodeEngineOptions) instead.");
            }
            else if (!string.Equals(previousEngine, _configuration[NodeHostDefaults.EngineKey], StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The engine name changed from \"{previousEngine}\" to \"{_configuration[NodeHostDefaults.EngineKey]}\". Changing the host configuration using NodeEngineBuilder.NodeHost is not supported. Use NodeEngine.CreateBuilder(NodeEngineOptions) instead.");
            }
            else if (!string.Equals(previousContentRootConfig, _configuration[NodeHostDefaults.ContentRootKey], StringComparison.OrdinalIgnoreCase)
                && !string.Equals(previousContentRoot, HostingPathResolver.ResolvePath(_configuration[NodeHostDefaults.ContentRootKey]), StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The content root changed from \"{previousContentRoot}\" to \"{HostingPathResolver.ResolvePath(_configuration[NodeHostDefaults.ContentRootKey])}\". Changing the host configuration using NodeEngineBuilder.NodeHost is not supported. Use NodeEngine.CreateBuilder(NodeEngineOptions) instead.");
            }
            else if (!string.Equals(previousEnvironment, _configuration[NodeHostDefaults.EnvironmentKey], StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The environment changed from \"{previousEnvironment}\" to \"{_configuration[NodeHostDefaults.EnvironmentKey]}\". Changing the host configuration using NodeEngineBuilder.NodeHost is not supported. Use NodeEngine.CreateBuilder(NodeEngineOptions) instead.");
            }
            else if (!string.Equals(previousHostingStartupAssemblies, _configuration[NodeHostDefaults.HostingStartupAssembliesKey], StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The hosting startup assemblies changed from \"{previousHostingStartupAssemblies}\" to \"{_configuration[NodeHostDefaults.HostingStartupAssembliesKey]}\". Changing the host configuration using NodeEngineBuilder.NodeHost is not supported. Use NodeEngine.CreateBuilder(NodeEngineOptions) instead.");
            }
            else if (!string.Equals(previousHostingStartupAssembliesExclude, _configuration[NodeHostDefaults.HostingStartupExcludeAssembliesKey], StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The hosting startup assemblies exclude list changed from \"{previousHostingStartupAssembliesExclude}\" to \"{_configuration[NodeHostDefaults.HostingStartupExcludeAssembliesKey]}\". Changing the host configuration using NodeEngineBuilder.NodeHost is not supported. Use NodeEngine.CreateBuilder(NodeEngineOptions) instead.");
            }

            return this;
        }

        /// <inheritdoc />
        public INodeHostBuilder ConfigureServices(Action<NodeHostBuilderContext, IServiceCollection> configureServices)
        {
            // Run these immediately so that they are observable by the imperative code
            configureServices(_context, _services);
            return this;
        }

        /// <inheritdoc />
        public INodeHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            return ConfigureServices((context, services) => configureServices(services));
        }

        /// <inheritdoc />
        public string GetSetting(string key)
        {
            return _configuration[key];
        }

        /// <inheritdoc />
        public INodeHostBuilder UseSetting(string key, string value)
        {
            // All properties on INodeHostEnvironment are non-nullable.
            if (value is null)
            {
                return this;
            }

            var previousContentRoot = HostingPathResolver.ResolvePath(_context.HostingEnvironment.ContentRootPath);
            var previousNodeRoot = HostingPathResolver.ResolvePath(_context.HostingEnvironment.NodeRootPath);
            var previousEngine = _configuration[NodeHostDefaults.EngineKey];
            var previousEnvironment = _configuration[NodeHostDefaults.EnvironmentKey];
            var previousHostingStartupAssemblies = _configuration[NodeHostDefaults.HostingStartupAssembliesKey];
            var previousHostingStartupAssembliesExclude = _configuration[NodeHostDefaults.HostingStartupExcludeAssembliesKey];

            if (string.Equals(key, NodeHostDefaults.NodeRootKey, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(HostingPathResolver.ResolvePath(previousNodeRoot, previousContentRoot), HostingPathResolver.ResolvePath(value, previousContentRoot), StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The node root changed from \"{HostingPathResolver.ResolvePath(previousNodeRoot, previousContentRoot)}\" to \"{HostingPathResolver.ResolvePath(value, previousContentRoot)}\". Changing the host configuration using NodeEngineBuilder.NodeHost is not supported. Use NodeEngine.CreateBuilder(NodeEngineOptions) instead.");
            }
            else if (string.Equals(key, NodeHostDefaults.EngineKey, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(previousEngine, value, StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The engine name changed from \"{previousEngine}\" to \"{value}\". Changing the host configuration using NodeEngineBuilder.NodeHost is not supported. Use NodeEngine.CreateBuilder(NodeEngineOptions) instead.");
            }
            else if (string.Equals(key, NodeHostDefaults.ContentRootKey, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(previousContentRoot, HostingPathResolver.ResolvePath(value), StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The content root changed from \"{previousContentRoot}\" to \"{HostingPathResolver.ResolvePath(value)}\". Changing the host configuration using NodeEngineBuilder.NodeHost is not supported. Use NodeEngine.CreateBuilder(NodeEngineOptions) instead.");
            }
            else if (string.Equals(key, NodeHostDefaults.EnvironmentKey, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(previousEnvironment, value, StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The environment changed from \"{previousEnvironment}\" to \"{value}\". Changing the host configuration using NodeEngineBuilder.NodeHost is not supported. Use NodeEngine.CreateBuilder(NodeEngineOptions) instead.");
            }
            else if (string.Equals(key, NodeHostDefaults.HostingStartupAssembliesKey, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(previousHostingStartupAssemblies, value, StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The hosting startup assemblies changed from \"{previousHostingStartupAssemblies}\" to \"{value}\". Changing the host configuration using NodeEngineBuilder.NodeHost is not supported. Use NodeEngine.CreateBuilder(NodeEngineOptions) instead.");
            }
            else if (string.Equals(key, NodeHostDefaults.HostingStartupExcludeAssembliesKey, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(previousHostingStartupAssembliesExclude, value, StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The hosting startup assemblies exclude list changed from \"{previousHostingStartupAssembliesExclude}\" to \"{value}\". Changing the host configuration using NodeEngineBuilder.NodeHost is not supported. Use NodeEngine.CreateBuilder(NodeEngineOptions) instead.");
            }

            // Set the configuration value after we've validated the key
            _configuration[key] = value;

            return this;
        }

        INodeHostBuilder ISupportsHostStartup.Configure(Action<IEngineBuilder> configure)
        {
            throw new NotSupportedException("Configure() is not supported by NodeEngineBuilder.NodeHost. Use the NodeEngine returned by NodeEngineBuilder.Build() instead.");
        }

        INodeHostBuilder ISupportsHostStartup.Configure(Action<NodeHostBuilderContext, IEngineBuilder> configure)
        {
            throw new NotSupportedException("Configure() is not supported by NodeEngineBuilder.NodeHost. Use the NodeEngine returned by NodeEngineBuilder.Build() instead.");
        }

        INodeHostBuilder ISupportsHostStartup.UseStartup([DynamicallyAccessedMembers(StartupLinkerOptions.Accessibility)] Type startupType)
        {
            throw new NotSupportedException("UseStartup() is not supported by NodeEngineBuilder.NodeHost. Use the NodeEngine returned by NodeEngineBuilder.Build() instead.");
        }

        INodeHostBuilder ISupportsHostStartup.UseStartup<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] TStartup>(Func<NodeHostBuilderContext, TStartup> startupFactory)
        {
            throw new NotSupportedException("UseStartup() is not supported by NodeEngineBuilder.NodeHost. Use the NodeEngine returned by NodeEngineBuilder.Build() instead.");
        }
    }
}
