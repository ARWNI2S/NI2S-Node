using ARWNI2S.Hosting;
using ARWNI2S.Hosting.Builder;
using ARWNI2S.Node.Builder;
using ARWNI2S.Node.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Node.Hosting
{
    /// <summary>
    /// A non-buildable <see cref="INiisHostBuilder"/> for <see cref="NodeHostBuilder"/>.
    /// Use <see cref="NodeHostBuilder.Build"/> to build the <see cref="NodeHostBuilder"/>.
    /// </summary>
    public sealed class ConfigureNI2SHostBuilder : INiisHostBuilder
    {
        private readonly INiisHostEnvironment _environment;
        private readonly ConfigurationManager _configuration;
        private readonly IServiceCollection _services;
        private readonly NI2SHostBuilderContext _context;

        internal ConfigureNI2SHostBuilder(NI2SHostBuilderContext niisHostBuilderContext, ConfigurationManager configuration, IServiceCollection services)
        {
            _configuration = configuration;
            _environment = niisHostBuilderContext.HostingEnvironment;
            _services = services;
            _context = niisHostBuilderContext;
        }

        INiisHost INiisHostBuilder.Build()
        {
            throw new NotSupportedException($"Call {nameof(NodeHostBuilder)}.{nameof(NodeHostBuilder.Build)}() instead.");
        }

        /// <inheritdoc />
        public INiisHostBuilder ConfigureNI2SConfiguration(Action<NI2SHostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            var previousContentRoot = HostingPathResolver.ResolvePath(_context.HostingEnvironment.ContentRootPath);
            var previousContentRootConfig = _configuration[NI2SHostingDefaults.ContentRootKey];
            var previousNiisRoot = HostingPathResolver.ResolvePath(_context.HostingEnvironment.NodeRootPath, previousContentRoot);
            var previousNiisRootConfig = _configuration[NI2SHostingDefaults.NodeRootKey];
            var previousApplication = _configuration[NI2SHostingDefaults.ApplicationKey];
            var previousEnvironment = _configuration[NI2SHostingDefaults.EnvironmentKey];

            // Run these immediately so that they are observable by the imperative code
            configureDelegate(_context, _configuration);

            if (!string.Equals(previousNiisRootConfig, _configuration[NI2SHostingDefaults.NodeRootKey], StringComparison.OrdinalIgnoreCase)
                && !string.Equals(HostingPathResolver.ResolvePath(previousNiisRoot, previousContentRoot), HostingPathResolver.ResolvePath(_configuration[NI2SHostingDefaults.NodeRootKey], previousContentRoot), StringComparison.OrdinalIgnoreCase))
            {
                // Diasllow changing the web root for consistency with other types.
                throw new NotSupportedException($"The web root changed from \"{HostingPathResolver.ResolvePath(previousNiisRoot, previousContentRoot)}\" to \"{HostingPathResolver.ResolvePath(_configuration[NI2SHostingDefaults.NodeRootKey], previousContentRoot)}\". Changing the host configuration using NI2SHostBuilder.NI2SHost is not supported. Use NI2SHost.CreateBuilder(NI2SNodeOptions) instead.");
            }
            else if (!string.Equals(previousApplication, _configuration[NI2SHostingDefaults.ApplicationKey], StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The application name changed from \"{previousApplication}\" to \"{_configuration[NI2SHostingDefaults.ApplicationKey]}\". Changing the host configuration using NI2SHostBuilder.NI2SHost is not supported. Use NI2SHost.CreateBuilder(NI2SNodeOptions) instead.");
            }
            else if (!string.Equals(previousContentRootConfig, _configuration[NI2SHostingDefaults.ContentRootKey], StringComparison.OrdinalIgnoreCase)
                && !string.Equals(previousContentRoot, HostingPathResolver.ResolvePath(_configuration[NI2SHostingDefaults.ContentRootKey]), StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The content root changed from \"{previousContentRoot}\" to \"{HostingPathResolver.ResolvePath(_configuration[NI2SHostingDefaults.ContentRootKey])}\". Changing the host configuration using NI2SHostBuilder.NI2SHost is not supported. Use NI2SHost.CreateBuilder(NI2SNodeOptions) instead.");
            }
            else if (!string.Equals(previousEnvironment, _configuration[NI2SHostingDefaults.EnvironmentKey], StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The environment changed from \"{previousEnvironment}\" to \"{_configuration[NI2SHostingDefaults.EnvironmentKey]}\". Changing the host configuration using NI2SHostBuilder.NI2SHost is not supported. Use NI2SHost.CreateBuilder(NI2SNodeOptions) instead.");
            }

            return this;
        }

        /// <inheritdoc />
        public INiisHostBuilder ConfigureServices(Action<NI2SHostBuilderContext, IServiceCollection> configureServices)
        {
            // Run these immediately so that they are observable by the imperative code
            configureServices(_context, _services);
            return this;
        }

        /// <inheritdoc />
        public INiisHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            return ConfigureServices((context, services) => configureServices(services));
        }

        /// <inheritdoc />
        public string GetSetting(string key)
        {
            return _configuration[key];
        }

        /// <inheritdoc />
        public INiisHostBuilder UseSetting(string key, string value)
        {
            // All properties on INiisHostEnvironment are non-nullable.
            if (value is null)
            {
                return this;
            }

            var previousContentRoot = HostingPathResolver.ResolvePath(_context.HostingEnvironment.ContentRootPath);
            var previousNiisRoot = HostingPathResolver.ResolvePath(_context.HostingEnvironment.NodeRootPath);
            var previousApplication = _configuration[NI2SHostingDefaults.ApplicationKey];
            var previousEnvironment = _configuration[NI2SHostingDefaults.EnvironmentKey];

            if (string.Equals(key, NI2SHostingDefaults.NodeRootKey, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(HostingPathResolver.ResolvePath(previousNiisRoot, previousContentRoot), HostingPathResolver.ResolvePath(value, previousContentRoot), StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The web root changed from \"{HostingPathResolver.ResolvePath(previousNiisRoot, previousContentRoot)}\" to \"{HostingPathResolver.ResolvePath(value, previousContentRoot)}\". Changing the host configuration using NI2SHostBuilder.NI2SHost is not supported. Use NI2SHost.CreateBuilder(NI2SNodeOptions) instead.");
            }
            else if (string.Equals(key, NI2SHostingDefaults.ApplicationKey, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(previousApplication, value, StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The application name changed from \"{previousApplication}\" to \"{value}\". Changing the host configuration using NI2SHostBuilder.NI2SHost is not supported. Use NI2SHost.CreateBuilder(NI2SNodeOptions) instead.");
            }
            else if (string.Equals(key, NI2SHostingDefaults.ContentRootKey, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(previousContentRoot, HostingPathResolver.ResolvePath(value), StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The content root changed from \"{previousContentRoot}\" to \"{HostingPathResolver.ResolvePath(value)}\". Changing the host configuration using NI2SHostBuilder.NI2SHost is not supported. Use NI2SHost.CreateBuilder(NI2SNodeOptions) instead.");
            }
            else if (string.Equals(key, NI2SHostingDefaults.EnvironmentKey, StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(previousEnvironment, value, StringComparison.OrdinalIgnoreCase))
            {
                // Disallow changing any host configuration
                throw new NotSupportedException($"The environment changed from \"{previousEnvironment}\" to \"{value}\". Changing the host configuration using NI2SHostBuilder.NI2SHost is not supported. Use NI2SHost.CreateBuilder(NI2SNodeOptions) instead.");
            }

            // Set the configuration value after we've validated the key
            _configuration[key] = value;

            return this;
        }
    }
}