using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Hosting.Infrastructure;
using NI2S.Node.Infrastructure;
using System;
using System.Diagnostics.CodeAnalysis;

namespace NI2S.Node.Hosting.Builder
{
    /// <summary>
    /// A non-buildable <see cref="INodeHostBuilder"/> for <see cref="NI2SNodeBuilder"/>.
    /// Use <see cref="NI2SNodeBuilder.Build"/> to build the <see cref="NI2SNodeBuilder"/>.
    /// </summary>
    public sealed class ConfigureNodeHostBuilder : INodeHostBuilder, ISupportsStartup
    {
        private readonly INodeHostEnvironment _environment;
        private readonly ConfigurationManager _configuration;
        private readonly IServiceCollection _services;
        private readonly NodeHostBuilderContext _context;

        internal ConfigureNodeHostBuilder(NodeHostBuilderContext webHostBuilderContext, ConfigurationManager configuration, IServiceCollection services)
        {
            _configuration = configuration;
            _environment = webHostBuilderContext.HostingEnvironment;
            _services = services;
            _context = webHostBuilderContext;
        }

        INodeHost INodeHostBuilder.Build()
        {
            throw new NotSupportedException($"Call {nameof(NI2SNodeBuilder)}.{nameof(NI2SNodeBuilder.Build)}() instead.");
        }

        /// <inheritdoc />
        public INodeHostBuilder ConfigureAppConfiguration(Action<NodeHostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            var previousContentRoot = HostingPathResolver.ResolvePath(_context.HostingEnvironment.ContentRootPath);
            var previousContentRootConfig = _configuration[NodeHostDefaults.ContentRootKey];
            var previousAssetsRoot = HostingPathResolver.ResolvePath(_context.HostingEnvironment.AssetsRootPath, previousContentRoot);
            var previousAssetsRootConfig = _configuration[NodeHostDefaults.AssetsRootKey];
            var previousApplication = _configuration[NodeHostDefaults.ApplicationKey];
            var previousEnvironment = _configuration[NodeHostDefaults.EnvironmentKey];
            var previousHostingStartupAssemblies = _configuration[NodeHostDefaults.HostingStartupAssembliesKey];
            var previousHostingStartupAssembliesExclude = _configuration[NodeHostDefaults.HostingStartupExcludeAssembliesKey];

            // Run these immediately so that they are observable by the imperative code
            configureDelegate(_context, _configuration);

            if (!string.Equals(previousAssetsRootConfig, _configuration[NodeHostDefaults.AssetsRootKey], StringComparison.OrdinalIgnoreCase)
                && !string.Equals(HostingPathResolver.ResolvePath(previousAssetsRoot, previousContentRoot), HostingPathResolver.ResolvePath(_configuration[NodeHostDefaults.AssetsRootKey], previousContentRoot), StringComparison.OrdinalIgnoreCase))
            {
                // Diasllow changing the web root for consistency with other types.
                throw new NotSupportedException($"The web root changed from \"{HostingPathResolver.ResolvePath(previousAssetsRoot, previousContentRoot)}\" to \"{HostingPathResolver.ResolvePath(_configuration[NodeHostDefaults.AssetsRootKey], previousContentRoot)}\". Changing the host configuration using NI2SNodeBuilder.NodeHost is not supported. Use NI2SNode.CreateBuilder(NI2SNodeOptions) instead.");
            }
            else if (!string.Equals(previousApplication, _configuration[NodeHostDefaults.ApplicationKey], StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The application name changed from \"{previousApplication}\" to \"{_configuration[NodeHostDefaults.ApplicationKey]}\". Changing the host configuration using NI2SNodeBuilder.NodeHost is not supported. Use NI2SNode.CreateBuilder(NI2SNodeOptions) instead.");
            }
            else if (!string.Equals(previousContentRootConfig, _configuration[NodeHostDefaults.ContentRootKey], StringComparison.OrdinalIgnoreCase)
                && !string.Equals(previousContentRoot, HostingPathResolver.ResolvePath(_configuration[NodeHostDefaults.ContentRootKey]), StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The content root changed from \"{previousContentRoot}\" to \"{HostingPathResolver.ResolvePath(_configuration[NodeHostDefaults.ContentRootKey])}\". Changing the host configuration using NI2SNodeBuilder.NodeHost is not supported. Use NI2SNode.CreateBuilder(NI2SNodeOptions) instead.");
            }
            else if (!string.Equals(previousEnvironment, _configuration[NodeHostDefaults.EnvironmentKey], StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The environment changed from \"{previousEnvironment}\" to \"{_configuration[NodeHostDefaults.EnvironmentKey]}\". Changing the host configuration using NI2SNodeBuilder.NodeHost is not supported. Use NI2SNode.CreateBuilder(NI2SNodeOptions) instead.");
            }
            else if (!string.Equals(previousHostingStartupAssemblies, _configuration[NodeHostDefaults.HostingStartupAssembliesKey], StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The hosting startup assemblies changed from \"{previousHostingStartupAssemblies}\" to \"{_configuration[NodeHostDefaults.HostingStartupAssembliesKey]}\". Changing the host configuration using NI2SNodeBuilder.NodeHost is not supported. Use NI2SNode.CreateBuilder(NI2SNodeOptions) instead.");
            }
            else if (!string.Equals(previousHostingStartupAssembliesExclude, _configuration[NodeHostDefaults.HostingStartupExcludeAssembliesKey], StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The hosting startup assemblies exclude list changed from \"{previousHostingStartupAssembliesExclude}\" to \"{_configuration[NodeHostDefaults.HostingStartupExcludeAssembliesKey]}\". Changing the host configuration using NI2SNodeBuilder.NodeHost is not supported. Use NI2SNode.CreateBuilder(NI2SNodeOptions) instead.");
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
            return ConfigureServices((NodeHostBuilderContext context, IServiceCollection services) => configureServices(services));
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
            var previousAssetsRoot = HostingPathResolver.ResolvePath(_context.HostingEnvironment.AssetsRootPath);
            var previousApplication = _configuration[NodeHostDefaults.ApplicationKey];
            var previousEnvironment = _configuration[NodeHostDefaults.EnvironmentKey];
            var previousHostingStartupAssemblies = _configuration[NodeHostDefaults.HostingStartupAssembliesKey];
            var previousHostingStartupAssembliesExclude = _configuration[NodeHostDefaults.HostingStartupExcludeAssembliesKey];

            if (string.Equals(key, NodeHostDefaults.AssetsRootKey, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(HostingPathResolver.ResolvePath(previousAssetsRoot, previousContentRoot), HostingPathResolver.ResolvePath(value, previousContentRoot), StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The web root changed from \"{HostingPathResolver.ResolvePath(previousAssetsRoot, previousContentRoot)}\" to \"{HostingPathResolver.ResolvePath(value, previousContentRoot)}\". Changing the host configuration using NI2SNodeBuilder.NodeHost is not supported. Use NI2SNode.CreateBuilder(NI2SNodeOptions) instead.");
            }
            else if (string.Equals(key, NodeHostDefaults.ApplicationKey, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(previousApplication, value, StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The application name changed from \"{previousApplication}\" to \"{value}\". Changing the host configuration using NI2SNodeBuilder.NodeHost is not supported. Use NI2SNode.CreateBuilder(NI2SNodeOptions) instead.");
            }
            else if (string.Equals(key, NodeHostDefaults.ContentRootKey, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(previousContentRoot, HostingPathResolver.ResolvePath(value), StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The content root changed from \"{previousContentRoot}\" to \"{HostingPathResolver.ResolvePath(value)}\". Changing the host configuration using NI2SNodeBuilder.NodeHost is not supported. Use NI2SNode.CreateBuilder(NI2SNodeOptions) instead.");
            }
            else if (string.Equals(key, NodeHostDefaults.EnvironmentKey, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(previousEnvironment, value, StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The environment changed from \"{previousEnvironment}\" to \"{value}\". Changing the host configuration using NI2SNodeBuilder.NodeHost is not supported. Use NI2SNode.CreateBuilder(NI2SNodeOptions) instead.");
            }
            else if (string.Equals(key, NodeHostDefaults.HostingStartupAssembliesKey, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(previousHostingStartupAssemblies, value, StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The hosting startup assemblies changed from \"{previousHostingStartupAssemblies}\" to \"{value}\". Changing the host configuration using NI2SNodeBuilder.NodeHost is not supported. Use NI2SNode.CreateBuilder(NI2SNodeOptions) instead.");
            }
            else if (string.Equals(key, NodeHostDefaults.HostingStartupExcludeAssembliesKey, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(previousHostingStartupAssembliesExclude, value, StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The hosting startup assemblies exclude list changed from \"{previousHostingStartupAssembliesExclude}\" to \"{value}\". Changing the host configuration using NI2SNodeBuilder.NodeHost is not supported. Use NI2SNode.CreateBuilder(NI2SNodeOptions) instead.");
            }

            // Set the configuration value after we've validated the key
            _configuration[key] = value;

            return this;
        }

        INodeHostBuilder ISupportsStartup.Configure(Action<INodeBuilder> configure)
        {
            throw new NotSupportedException("Configure() is not supported by NI2SNodeBuilder.NodeHost. Use the NI2SNode returned by NI2SNodeBuilder.Build() instead.");
        }

        INodeHostBuilder ISupportsStartup.Configure(Action<NodeHostBuilderContext, INodeBuilder> configure)
        {
            throw new NotSupportedException("Configure() is not supported by NI2SNodeBuilder.NodeHost. Use the NI2SNode returned by NI2SNodeBuilder.Build() instead.");
        }

        INodeHostBuilder ISupportsStartup.UseStartup([DynamicallyAccessedMembers(StartupLinkerOptions.Accessibility)] Type startupType)
        {
            throw new NotSupportedException("UseStartup() is not supported by NI2SNodeBuilder.NodeHost. Use the NI2SNode returned by NI2SNodeBuilder.Build() instead.");
        }

        INodeHostBuilder ISupportsStartup.UseStartup<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] TStartup>(Func<NodeHostBuilderContext, TStartup> startupFactory)
        {
            throw new NotSupportedException("UseStartup() is not supported by NI2SNodeBuilder.NodeHost. Use the NI2SNode returned by NI2SNodeBuilder.Build() instead.");
        }
    }
}
