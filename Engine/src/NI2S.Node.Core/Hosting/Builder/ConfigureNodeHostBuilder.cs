// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Hosting.Infrastructure;
using System;
using System.Diagnostics.CodeAnalysis;

namespace NI2S.Node.Hosting.Builder
{
    /// <summary>
    /// A non-buildable <see cref="INodeHostBuilder"/> for <see cref="NodeEngineHostBuilder"/>.
    /// Use <see cref="NodeEngineHostBuilder.Build"/> to build the <see cref="NodeEngineHostBuilder"/>.
    /// </summary>
    public sealed class ConfigureNodeHostBuilder : INodeHostBuilder, ISupportsStartup
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
            throw new NotSupportedException($"Call {nameof(NodeEngineHostBuilder)}.{nameof(NodeEngineHostBuilder.Build)}() instead.");
        }

        /// <inheritdoc />
        public INodeHostBuilder ConfigureNodeConfiguration(Action<NodeHostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            var previousContentRoot = PathResolver.ResolvePath(_context.HostingEnvironment.ContentRootPath);
            var previousContentRootConfig = _configuration[NodeHostDefaults.ContentRootKey];
            var previousNodeRoot = PathResolver.ResolvePath(_context.HostingEnvironment.NodeRootPath, previousContentRoot);
            var previousNodeRootConfig = _configuration[NodeHostDefaults.NodeRootKey];
            var previousEngine = _configuration[NodeHostDefaults.ApplicationKey];
            var previousEnvironment = _configuration[NodeHostDefaults.EnvironmentKey];
            var previousHostingStartupAssemblies = _configuration[NodeHostDefaults.HostingStartupAssembliesKey];
            var previousHostingStartupAssembliesExclude = _configuration[NodeHostDefaults.HostingStartupExcludeAssembliesKey];

            // Run these immediately so that they are observable by the imperative code
            configureDelegate(_context, _configuration);

            if (!string.Equals(previousNodeRootConfig, _configuration[NodeHostDefaults.NodeRootKey], StringComparison.OrdinalIgnoreCase)
                && !string.Equals(PathResolver.ResolvePath(previousNodeRoot, previousContentRoot), PathResolver.ResolvePath(_configuration[NodeHostDefaults.NodeRootKey], previousContentRoot), StringComparison.OrdinalIgnoreCase))
            {
                // Diasllow changing the web root for consistency with other types.
                throw new NotSupportedException($"The web root changed from \"{PathResolver.ResolvePath(previousNodeRoot, previousContentRoot)}\" to \"{PathResolver.ResolvePath(_configuration[NodeHostDefaults.NodeRootKey], previousContentRoot)}\". Changing the host configuration using EngineBuilder.NodeHost is not supported. Use NodeEngine.CreateBuilder(NodeEngineOptions) instead.");
            }
            else if (!string.Equals(previousEngine, _configuration[NodeHostDefaults.ApplicationKey], StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The engine name changed from \"{previousEngine}\" to \"{_configuration[NodeHostDefaults.ApplicationKey]}\". Changing the host configuration using EngineBuilder.NodeHost is not supported. Use NodeEngine.CreateBuilder(NodeEngineOptions) instead.");
            }
            else if (!string.Equals(previousContentRootConfig, _configuration[NodeHostDefaults.ContentRootKey], StringComparison.OrdinalIgnoreCase)
                && !string.Equals(previousContentRoot, PathResolver.ResolvePath(_configuration[NodeHostDefaults.ContentRootKey]), StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The content root changed from \"{previousContentRoot}\" to \"{PathResolver.ResolvePath(_configuration[NodeHostDefaults.ContentRootKey])}\". Changing the host configuration using EngineBuilder.NodeHost is not supported. Use NodeEngine.CreateBuilder(NodeEngineOptions) instead.");
            }
            else if (!string.Equals(previousEnvironment, _configuration[NodeHostDefaults.EnvironmentKey], StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The environment changed from \"{previousEnvironment}\" to \"{_configuration[NodeHostDefaults.EnvironmentKey]}\". Changing the host configuration using EngineBuilder.NodeHost is not supported. Use NodeEngine.CreateBuilder(NodeEngineOptions) instead.");
            }
            else if (!string.Equals(previousHostingStartupAssemblies, _configuration[NodeHostDefaults.HostingStartupAssembliesKey], StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The hosting startup assemblies changed from \"{previousHostingStartupAssemblies}\" to \"{_configuration[NodeHostDefaults.HostingStartupAssembliesKey]}\". Changing the host configuration using EngineBuilder.NodeHost is not supported. Use NodeEngine.CreateBuilder(NodeEngineOptions) instead.");
            }
            else if (!string.Equals(previousHostingStartupAssembliesExclude, _configuration[NodeHostDefaults.HostingStartupExcludeAssembliesKey], StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The hosting startup assemblies exclude list changed from \"{previousHostingStartupAssembliesExclude}\" to \"{_configuration[NodeHostDefaults.HostingStartupExcludeAssembliesKey]}\". Changing the host configuration using EngineBuilder.NodeHost is not supported. Use NodeEngine.CreateBuilder(NodeEngineOptions) instead.");
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

            var previousContentRoot = PathResolver.ResolvePath(_context.HostingEnvironment.ContentRootPath);
            var previousNodeRoot = PathResolver.ResolvePath(_context.HostingEnvironment.NodeRootPath);
            var previousEngine = _configuration[NodeHostDefaults.ApplicationKey];
            var previousEnvironment = _configuration[NodeHostDefaults.EnvironmentKey];
            var previousHostingStartupAssemblies = _configuration[NodeHostDefaults.HostingStartupAssembliesKey];
            var previousHostingStartupAssembliesExclude = _configuration[NodeHostDefaults.HostingStartupExcludeAssembliesKey];

            if (string.Equals(key, NodeHostDefaults.NodeRootKey, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(PathResolver.ResolvePath(previousNodeRoot, previousContentRoot), PathResolver.ResolvePath(value, previousContentRoot), StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The web root changed from \"{PathResolver.ResolvePath(previousNodeRoot, previousContentRoot)}\" to \"{PathResolver.ResolvePath(value, previousContentRoot)}\". Changing the host configuration using EngineBuilder.NodeHost is not supported. Use NodeEngine.CreateBuilder(NodeEngineOptions) instead.");
            }
            else if (string.Equals(key, NodeHostDefaults.ApplicationKey, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(previousEngine, value, StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The engine name changed from \"{previousEngine}\" to \"{value}\". Changing the host configuration using EngineBuilder.NodeHost is not supported. Use NodeEngine.CreateBuilder(NodeEngineOptions) instead.");
            }
            else if (string.Equals(key, NodeHostDefaults.ContentRootKey, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(previousContentRoot, PathResolver.ResolvePath(value), StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The content root changed from \"{previousContentRoot}\" to \"{PathResolver.ResolvePath(value)}\". Changing the host configuration using EngineBuilder.NodeHost is not supported. Use NodeEngine.CreateBuilder(NodeEngineOptions) instead.");
            }
            else if (string.Equals(key, NodeHostDefaults.EnvironmentKey, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(previousEnvironment, value, StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The environment changed from \"{previousEnvironment}\" to \"{value}\". Changing the host configuration using EngineBuilder.NodeHost is not supported. Use NodeEngine.CreateBuilder(NodeEngineOptions) instead.");
            }
            else if (string.Equals(key, NodeHostDefaults.HostingStartupAssembliesKey, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(previousHostingStartupAssemblies, value, StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The hosting startup assemblies changed from \"{previousHostingStartupAssemblies}\" to \"{value}\". Changing the host configuration using EngineBuilder.NodeHost is not supported. Use NodeEngine.CreateBuilder(NodeEngineOptions) instead.");
            }
            else if (string.Equals(key, NodeHostDefaults.HostingStartupExcludeAssembliesKey, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(previousHostingStartupAssembliesExclude, value, StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The hosting startup assemblies exclude list changed from \"{previousHostingStartupAssembliesExclude}\" to \"{value}\". Changing the host configuration using EngineBuilder.NodeHost is not supported. Use NodeEngine.CreateBuilder(NodeEngineOptions) instead.");
            }

            // Set the configuration value after we've validated the key
            _configuration[key] = value;

            return this;
        }

        INodeHostBuilder ISupportsStartup.Configure(Action<IEngineBuilder> configure)
        {
            throw new NotSupportedException("Configure() is not supported by EngineBuilder.NodeHost. Use the NodeEngine returned by EngineBuilder.Build() instead.");
        }

        INodeHostBuilder ISupportsStartup.Configure(Action<NodeHostBuilderContext, IEngineBuilder> configure)
        {
            throw new NotSupportedException("Configure() is not supported by EngineBuilder.NodeHost. Use the NodeEngine returned by EngineBuilder.Build() instead.");
        }

        INodeHostBuilder ISupportsStartup.UseStartup([DynamicallyAccessedMembers(StartupLinkerOptions.Accessibility)] Type startupType)
        {
            throw new NotSupportedException("UseStartup() is not supported by EngineBuilder.NodeHost. Use the NodeEngine returned by EngineBuilder.Build() instead.");
        }

        INodeHostBuilder ISupportsStartup.UseStartup<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] TStartup>(Func<NodeHostBuilderContext, TStartup> startupFactory)
        {
            throw new NotSupportedException("UseStartup() is not supported by EngineBuilder.NodeHost. Use the NodeEngine returned by EngineBuilder.Build() instead.");
        }
    }
}