using ARWNI2S.Hosting.Builder;

namespace ARWNI2S.Hosting
{
    /// <summary>
    /// A non-buildable <see cref="IHostBuilder"/> for <see cref="NI2SHostBuilder"/>.
    /// Use <see cref="NI2SHostBuilder.Build"/> to build the <see cref="NI2SHostBuilder"/>.
    /// </summary>
    public sealed class ConfigureHostBuilder : IHostBuilder//TODO:, ISupportsConfigureNiisHost remove or add?
    {
        private readonly ConfigurationManager _configuration;
        private readonly IServiceCollection _services;
        private readonly HostBuilderContext _context;

        private readonly List<Action<HostBuilderContext, object>> _configureContainerActions = [];
        private IServiceProviderFactory<object> _serviceProviderFactory;

        internal ConfigureHostBuilder(
            HostBuilderContext context,
            ConfigurationManager configuration,
            IServiceCollection services)
        {
            _configuration = configuration;
            _services = services;
            _context = context;
        }

        /// <inheritdoc />
        public IDictionary<object, object> Properties => _context.Properties;

        IHost IHostBuilder.Build()
        {
            throw new NotSupportedException($"Call {nameof(NI2SHostBuilder)}.{nameof(NI2SHostBuilder.Build)}() instead.");
        }

        /// <inheritdoc />
        public IHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            // Run these immediately so that they are observable by the imperative code
            configureDelegate(_context, _configuration);
            return this;
        }

        /// <inheritdoc />
        public IHostBuilder ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configureDelegate)
        {
            ArgumentNullException.ThrowIfNull(configureDelegate);

            _configureContainerActions.Add((context, containerBuilder) => configureDelegate(context, (TContainerBuilder)containerBuilder));

            return this;
        }

        /// <inheritdoc />
        public IHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
        {
            var previousApplicationName = _configuration[HostDefaults.ApplicationKey];
            // Use the real content root so we can compare paths
            var previousContentRoot = HostingPathResolver.ResolvePath(_context.HostingEnvironment.ContentRootPath);
            var previousContentRootConfig = _configuration[HostDefaults.ContentRootKey];
            var previousEnvironment = _configuration[HostDefaults.EnvironmentKey];

            // Run these immediately so that they are observable by the imperative code
            configureDelegate(_configuration);

            // Disallow changing any host settings this late in the cycle, the reasoning is that we've already loaded the default configuration
            // and done other things based on environment name, application name or content root.
            if (!string.Equals(previousApplicationName, _configuration[HostDefaults.ApplicationKey], StringComparison.OrdinalIgnoreCase))
            {
                throw new NotSupportedException($"The application name changed from \"{previousApplicationName}\" to \"{_configuration[HostDefaults.ApplicationKey]}\". Changing the host configuration using NI2SHostBuilder.Host is not supported. Use NI2SHost.CreateBuilder(NI2SNodeOptions) instead.");
            }

            if (!string.Equals(previousContentRootConfig, _configuration[HostDefaults.ContentRootKey], StringComparison.OrdinalIgnoreCase)
                && !string.Equals(previousContentRoot, HostingPathResolver.ResolvePath(_configuration[HostDefaults.ContentRootKey]), StringComparison.OrdinalIgnoreCase))
            {
                throw new NotSupportedException($"The content root changed from \"{previousContentRoot}\" to \"{HostingPathResolver.ResolvePath(_configuration[HostDefaults.ContentRootKey])}\". Changing the host configuration using NI2SHostBuilder.Host is not supported. Use NI2SHost.CreateBuilder(NI2SNodeOptions) instead.");
            }

            if (!string.Equals(previousEnvironment, _configuration[HostDefaults.EnvironmentKey], StringComparison.OrdinalIgnoreCase))
            {
                throw new NotSupportedException($"The environment changed from \"{previousEnvironment}\" to \"{_configuration[HostDefaults.EnvironmentKey]}\". Changing the host configuration using NI2SHostBuilder.Host is not supported. Use NI2SHost.CreateBuilder(NI2SNodeOptions) instead.");
            }

            return this;
        }

        /// <inheritdoc />
        public IHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            // Run these immediately so that they are observable by the imperative code
            configureDelegate(_context, _services);
            return this;
        }

        /// <inheritdoc />
        public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory) where TContainerBuilder : notnull
        {
            ArgumentNullException.ThrowIfNull(factory);

            _serviceProviderFactory = new ServiceProviderFactoryAdapter<TContainerBuilder>(factory);
            return this;
        }

        /// <inheritdoc />
        public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory) where TContainerBuilder : notnull
        {
            return UseServiceProviderFactory(factory(_context));
        }

        //IHostBuilder ISupportsConfigureNiisHost.ConfigureNI2SHost(Action<INiisHostBuilder> configure, Action<NI2SNodeOptions> configureOptions)
        //{
        //    throw new NotSupportedException("ConfigureNI2SHost() is not supported by NI2SHostBuilder.Host. Use the NI2SHost returned by NI2SHostBuilder.Build() instead.");
        //}

        internal void ApplyServiceProviderFactory(HostApplicationBuilder hostApplicationBuilder)
        {
            if (_serviceProviderFactory is null)
            {
                var niisSettings = Singleton<NI2SSettings>.Instance;
                var useAutofac = niisSettings.Get<CommonConfig>().UseAutofac;

                if (useAutofac)
                    UseServiceProviderFactory(new AutofacServiceProviderFactory());
                else
                    this.UseDefaultServiceProvider(options =>
                    {
                        //we don't validate the scopes, since at the app start and the initial configuration we need 
                        //to resolve some services (registered as "scoped") through the root container
                        options.ValidateScopes = false;
                        options.ValidateOnBuild = true;
                    });
            }

            void ConfigureContainerBuilderAdapter(object containerBuilder)
            {
                foreach (var action in _configureContainerActions)
                {
                    action(_context, containerBuilder);
                }
            }

            hostApplicationBuilder.ConfigureContainer(_serviceProviderFactory, ConfigureContainerBuilderAdapter);
        }

        private sealed class ServiceProviderFactoryAdapter<TContainerBuilder> : IServiceProviderFactory<object> where TContainerBuilder : notnull
        {
            private readonly IServiceProviderFactory<TContainerBuilder> _serviceProviderFactory;

            public ServiceProviderFactoryAdapter(IServiceProviderFactory<TContainerBuilder> serviceProviderFactory)
            {
                _serviceProviderFactory = serviceProviderFactory;
            }

            public object CreateBuilder(IServiceCollection services) => _serviceProviderFactory.CreateBuilder(services);
            public IServiceProvider CreateServiceProvider(object containerBuilder) => _serviceProviderFactory.CreateServiceProvider((TContainerBuilder)containerBuilder);
        }
    }
}