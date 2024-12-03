
using ARWNI2S.Configuration;
using ARWNI2S.Engine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ARWNI2S.Hosting
{
    //
    // Resumen:
    //     Represents a hosted applications and services builder that helps manage configuration,
    //     logging, lifetime, and more.
    public sealed class HostApplicationBuilder : IHostApplicationBuilder
    {
        private sealed class HostBuilderAdapter : IHostBuilder
        {
            private readonly HostApplicationBuilder _hostApplicationBuilder;

            private readonly List<Action<IConfigurationBuilder>> _configureHostConfigActions = new List<Action<IConfigurationBuilder>>();

            private readonly List<Action<HostBuilderContext, IConfigurationBuilder>> _configureAppConfigActions = new List<Action<HostBuilderContext, IConfigurationBuilder>>();

            private readonly List<IConfigureContainerAdapter> _configureContainerActions = new List<IConfigureContainerAdapter>();

            private readonly List<Action<HostBuilderContext, IServiceCollection>> _configureServicesActions = new List<Action<HostBuilderContext, IServiceCollection>>();

            private IServiceFactoryAdapter _serviceProviderFactory;

            public IDictionary<object, object> Properties => _hostApplicationBuilder._hostBuilderContext.Properties;

            public HostBuilderAdapter(HostApplicationBuilder hostApplicationBuilder)
            {
                _hostApplicationBuilder = hostApplicationBuilder;
            }

            public void ApplyChanges()
            {
                ConfigurationManager configuration = _hostApplicationBuilder.Configuration;
                if (_configureHostConfigActions.Count > 0)
                {
                    string text = configuration[HostDefaults.ApplicationKey];
                    string text2 = configuration[HostDefaults.EnvironmentKey];
                    string text3 = configuration[HostDefaults.ContentRootKey];
                    string contentRootPath = _hostApplicationBuilder._hostBuilderContext.HostingEnvironment.ContentRootPath;
                    foreach (Action<IConfigurationBuilder> configureHostConfigAction in _configureHostConfigActions)
                    {
                        configureHostConfigAction(configuration);
                    }

                    if (!string.Equals(text, configuration[HostDefaults.ApplicationKey], StringComparison.OrdinalIgnoreCase))
                    {
                        throw new NotSupportedException(System.SR.Format(System.SR.ApplicationNameChangeNotSupported, text, configuration[HostDefaults.ApplicationKey]));
                    }

                    if (!string.Equals(text2, configuration[HostDefaults.EnvironmentKey], StringComparison.OrdinalIgnoreCase))
                    {
                        throw new NotSupportedException(System.SR.Format(System.SR.EnvironmentNameChangeNotSupoprted, text2, configuration[HostDefaults.EnvironmentKey]));
                    }

                    string text4 = configuration[HostDefaults.ContentRootKey];
                    if (!string.Equals(text3, text4, StringComparison.OrdinalIgnoreCase) && !string.Equals(contentRootPath, HostBuilder.ResolveContentRootPath(text4, AppContext.BaseDirectory), StringComparison.OrdinalIgnoreCase))
                    {
                        throw new NotSupportedException(System.SR.Format(System.SR.ContentRootChangeNotSupported, text3, text4));
                    }
                }

                foreach (Action<HostBuilderContext, IConfigurationBuilder> configureAppConfigAction in _configureAppConfigActions)
                {
                    configureAppConfigAction(_hostApplicationBuilder._hostBuilderContext, configuration);
                }

                foreach (Action<HostBuilderContext, IServiceCollection> configureServicesAction in _configureServicesActions)
                {
                    configureServicesAction(_hostApplicationBuilder._hostBuilderContext, _hostApplicationBuilder.Services);
                }

                if (_configureContainerActions.Count > 0)
                {
                    Action<object> previousConfigureContainer = _hostApplicationBuilder._configureContainer;
                    _hostApplicationBuilder._configureContainer = delegate (object containerBuilder)
                    {
                        previousConfigureContainer(containerBuilder);
                        foreach (IConfigureContainerAdapter configureContainerAction in _configureContainerActions)
                        {
                            configureContainerAction.ConfigureContainer(_hostApplicationBuilder._hostBuilderContext, containerBuilder);
                        }
                    };
                }

                if (_serviceProviderFactory != null)
                {
                    _hostApplicationBuilder._createServiceProvider = delegate
                    {
                        object obj = _serviceProviderFactory.CreateBuilder(_hostApplicationBuilder.Services);
                        _hostApplicationBuilder._configureContainer(obj);
                        return _serviceProviderFactory.CreateServiceProvider(obj);
                    };
                }
            }

            public IHost Build()
            {
                throw new NotSupportedException();
            }

            public IHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
            {
                System.ThrowHelper.ThrowIfNull(configureDelegate, "configureDelegate");
                _configureHostConfigActions.Add(configureDelegate);
                return this;
            }

            public IHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
            {
                System.ThrowHelper.ThrowIfNull(configureDelegate, "configureDelegate");
                _configureAppConfigActions.Add(configureDelegate);
                return this;
            }

            public IHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
            {
                System.ThrowHelper.ThrowIfNull(configureDelegate, "configureDelegate");
                _configureServicesActions.Add(configureDelegate);
                return this;
            }

            public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory)
            {
                System.ThrowHelper.ThrowIfNull(factory, "factory");
                _serviceProviderFactory = new ServiceFactoryAdapter<TContainerBuilder>(factory);
                return this;
            }

            public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory)
            {
                System.ThrowHelper.ThrowIfNull(factory, "factory");
                _serviceProviderFactory = new ServiceFactoryAdapter<TContainerBuilder>(() => _hostApplicationBuilder._hostBuilderContext, factory);
                return this;
            }

            public IHostBuilder ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configureDelegate)
            {
                System.ThrowHelper.ThrowIfNull(configureDelegate, "configureDelegate");
                _configureContainerActions.Add(new ConfigureContainerAdapter<TContainerBuilder>(configureDelegate));
                return this;
            }
        }

        private sealed class LoggingBuilder : ILoggingBuilder
        {
            public IServiceCollection Services { get; }

            public LoggingBuilder(IServiceCollection services)
            {
                Services = services;
            }
        }

        private sealed class MetricsBuilder : IMetricsBuilder
        {
            public IServiceCollection Services { get; }

            public MetricsBuilder(IServiceCollection services)
            {
                Services = services;
                base._002Ector();
            }
        }

        private readonly HostBuilderContext _hostBuilderContext;

        private readonly ServiceCollection _serviceCollection = new ServiceCollection();

        private readonly IHostEnvironment _environment;

        private readonly LoggingBuilder _logging;

        private readonly MetricsBuilder _metrics;

        private Func<IServiceProvider> _createServiceProvider;

        private Action<object> _configureContainer = delegate
        {
        };

        private HostBuilderAdapter _hostBuilderAdapter;

        private IServiceProvider _appServices;

        private bool _hostBuilt;

        IDictionary<object, object> IHostApplicationBuilder.Properties => _hostBuilderContext.Properties;

        public IHostEnvironment Environment => _environment;

        //
        // Resumen:
        //     Gets the set of key/value configuration properties.
        //
        // Comentarios:
        //     This can be mutated by adding more configuration sources, which will update its
        //     current view.
        public ConfigurationManager Configuration { get; }

        IConfigurationManager IHostApplicationBuilder.Configuration => Configuration;

        public IServiceCollection Services => _serviceCollection;

        public ILoggingBuilder Logging => _logging;

        public IMetricsBuilder Metrics => _metrics;

        //
        // Resumen:
        //     Initializes a new instance of the Microsoft.Extensions.Hosting.HostApplicationBuilder
        //     class with preconfigured defaults.
        //
        // Comentarios:
        //     The following defaults are applied to the returned Microsoft.Extensions.Hosting.HostApplicationBuilder:
        //
        //
        //     • set the Microsoft.Extensions.Hosting.IHostEnvironment.ContentRootPath to the
        //     result of System.IO.Directory.GetCurrentDirectory
        //     • load host Microsoft.Extensions.Configuration.IConfiguration from "DOTNET_"
        //     prefixed environment variables
        //     • load host Microsoft.Extensions.Configuration.IConfiguration from supplied command
        //     line args
        //     • load app Microsoft.Extensions.Configuration.IConfiguration from 'appsettings.json'
        //     and 'appsettings.[Microsoft.Extensions.Hosting.IHostEnvironment.EnvironmentName].json'
        //
        //     • load app Microsoft.Extensions.Configuration.IConfiguration from User Secrets
        //     when Microsoft.Extensions.Hosting.IHostEnvironment.EnvironmentName is 'Development'
        //     using the entry assembly
        //     • load app Microsoft.Extensions.Configuration.IConfiguration from environment
        //     variables
        //     • load app Microsoft.Extensions.Configuration.IConfiguration from supplied command
        //     line args
        //     • configure the Microsoft.Extensions.Logging.ILoggerFactory to log to the console,
        //     debug, and event source output
        //     • enables scope validation on the dependency injection container when Microsoft.Extensions.Hosting.IHostEnvironment.EnvironmentName
        //     is 'Development'
        public HostApplicationBuilder()
            : this((string[]?)null)
        {
        }

        //
        // Resumen:
        //     Initializes a new instance of the Microsoft.Extensions.Hosting.HostApplicationBuilder
        //     class with preconfigured defaults.
        //
        // Parámetros:
        //   args:
        //     The command line args.
        //
        // Comentarios:
        //     The following defaults are applied to the returned Microsoft.Extensions.Hosting.HostApplicationBuilder:
        //
        //
        //     • set the Microsoft.Extensions.Hosting.IHostEnvironment.ContentRootPath to the
        //     result of System.IO.Directory.GetCurrentDirectory
        //     • load host Microsoft.Extensions.Configuration.IConfiguration from "DOTNET_"
        //     prefixed environment variables
        //     • load host Microsoft.Extensions.Configuration.IConfiguration from supplied command
        //     line args
        //     • load app Microsoft.Extensions.Configuration.IConfiguration from 'appsettings.json'
        //     and 'appsettings.[Microsoft.Extensions.Hosting.IHostEnvironment.EnvironmentName].json'
        //
        //     • load app Microsoft.Extensions.Configuration.IConfiguration from User Secrets
        //     when Microsoft.Extensions.Hosting.IHostEnvironment.EnvironmentName is 'Development'
        //     using the entry assembly
        //     • load app Microsoft.Extensions.Configuration.IConfiguration from environment
        //     variables
        //     • load app Microsoft.Extensions.Configuration.IConfiguration from supplied command
        //     line args
        //     • configure the Microsoft.Extensions.Logging.ILoggerFactory to log to the console,
        //     debug, and event source output
        //     • enables scope validation on the dependency injection container when Microsoft.Extensions.Hosting.IHostEnvironment.EnvironmentName
        //     is 'Development'
        public HostApplicationBuilder(string[]? args)
            : this(new HostApplicationBuilderSettings
            {
                Args = args
            })
        {
        }

        //
        // Resumen:
        //     Initializes a new instance of the Microsoft.Extensions.Hosting.HostApplicationBuilder.
        //
        //
        // Parámetros:
        //   settings:
        //     Settings controlling initial configuration and whether default settings should
        //     be used.
        public HostApplicationBuilder(HostApplicationBuilderSettings? settings)
        {
            HostApplicationBuilder hostApplicationBuilder = this;
            if (settings == null)
            {
                settings = new HostApplicationBuilderSettings();
            }

            Configuration = settings.Configuration ?? new ConfigurationManager();
            if (!settings.DisableDefaults)
            {
                if (settings.ContentRootPath == null && Configuration[HostDefaults.ContentRootKey] == null)
                {
                    HostingHostBuilderExtensions.SetDefaultContentRoot(Configuration);
                }

                Configuration.AddEnvironmentVariables("DOTNET_");
            }

            Initialize(settings, out _hostBuilderContext, out _environment, out _logging, out _metrics);
            ServiceProviderOptions serviceProviderOptions = null;
            if (!settings.DisableDefaults)
            {
                HostingHostBuilderExtensions.ApplyDefaultAppConfiguration(_hostBuilderContext, Configuration, settings.Args);
                HostingHostBuilderExtensions.AddDefaultServices(_hostBuilderContext, Services);
                serviceProviderOptions = HostingHostBuilderExtensions.CreateDefaultServiceProviderOptions(_hostBuilderContext);
            }

            _createServiceProvider = delegate
            {
                hostApplicationBuilder._configureContainer(hostApplicationBuilder.Services);
                return (serviceProviderOptions != null) ? hostApplicationBuilder.Services.BuildServiceProvider(serviceProviderOptions) : hostApplicationBuilder.Services.BuildServiceProvider();
            };
        }

        internal HostApplicationBuilder(HostApplicationBuilderSettings settings, bool empty)
        {
            if (settings == null)
            {
                settings = new HostApplicationBuilderSettings();
            }

            Configuration = settings.Configuration ?? new ConfigurationManager();
            Initialize(settings, out _hostBuilderContext, out _environment, out _logging, out _metrics);
            _createServiceProvider = delegate
            {
                _configureContainer(Services);
                return Services.BuildServiceProvider();
            };
        }

        private void Initialize(HostApplicationBuilderSettings settings, out HostBuilderContext hostBuilderContext, out IHostEnvironment environment, out LoggingBuilder logging, out MetricsBuilder metrics)
        {
            HostingHostBuilderExtensions.AddCommandLineConfig(Configuration, settings.Args);
            List<KeyValuePair<string, string>> list = null;
            if (settings.ApplicationName != null)
            {
                if (list == null)
                {
                    list = new List<KeyValuePair<string, string>>();
                }

                list.Add(new KeyValuePair<string, string>(HostDefaults.ApplicationKey, settings.ApplicationName));
            }

            if (settings.EnvironmentName != null)
            {
                if (list == null)
                {
                    list = new List<KeyValuePair<string, string>>();
                }

                list.Add(new KeyValuePair<string, string>(HostDefaults.EnvironmentKey, settings.EnvironmentName));
            }

            if (settings.ContentRootPath != null)
            {
                if (list == null)
                {
                    list = new List<KeyValuePair<string, string>>();
                }

                list.Add(new KeyValuePair<string, string>(HostDefaults.ContentRootKey, settings.ContentRootPath));
            }

            if (list != null)
            {
                Configuration.AddInMemoryCollection(list);
            }

            var (hostingEnvironment, physicalFileProvider) = HostBuilder.CreateHostingEnvironment(Configuration);
            Configuration.SetFileProvider(physicalFileProvider);
            hostBuilderContext = new HostBuilderContext(new Dictionary<object, object>())
            {
                HostingEnvironment = hostingEnvironment,
                Configuration = Configuration
            };
            environment = hostingEnvironment;
            HostBuilder.PopulateServiceCollection(Services, hostBuilderContext, hostingEnvironment, physicalFileProvider, Configuration, () => _appServices);
            logging = new LoggingBuilder(Services);
            metrics = new MetricsBuilder(Services);
        }

        public void ConfigureContainer<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory, Action<TContainerBuilder>? configure = null) where TContainerBuilder : notnull
        {
            IServiceProviderFactory<TContainerBuilder> factory2 = factory;
            Action<TContainerBuilder> configure2 = configure;
            _createServiceProvider = delegate
            {
                TContainerBuilder val = factory2.CreateBuilder(Services);
                _configureContainer(val);
                return factory2.CreateServiceProvider(val);
            };
            _configureContainer = delegate (object containerBuilder)
            {
                configure2?.Invoke((TContainerBuilder)containerBuilder);
            };
        }

        //
        // Resumen:
        //     Builds the host. This method can only be called once.
        //
        // Devuelve:
        //     An initialized Microsoft.Extensions.Hosting.IHost.
        public IHost Build()
        {
            if (_hostBuilt)
            {
                throw new InvalidOperationException(System.SR.BuildCalled);
            }

            _hostBuilt = true;
            using DiagnosticListener diagnosticListener = HostBuilder.LogHostBuilding(this);
            _hostBuilderAdapter?.ApplyChanges();
            _appServices = _createServiceProvider();
            _serviceCollection.MakeReadOnly();
            return HostBuilder.ResolveHost(_appServices, diagnosticListener);
        }

        internal IHostBuilder AsHostBuilder()
        {
            return _hostBuilderAdapter ?? (_hostBuilderAdapter = new HostBuilderAdapter(this));
        }
    }

    internal class NiisNodeBuilder
    {
        private readonly HostApplicationBuilder _appBuilder;
        public NiisNodeBuilder(string[] args)
        {
            _appBuilder = new HostApplicationBuilder(args);

            _appBuilder.Configuration.AddJsonFile(ConfigurationDefaults.SettingsFilePath, true, true);
            if (!string.IsNullOrEmpty(_appBuilder.Environment?.EnvironmentName))
            {
                var path = string.Format(ConfigurationDefaults.SettingsEnvironmentFilePath, _appBuilder.Environment.EnvironmentName);
                _appBuilder.Configuration.AddJsonFile(path, true, true);
            }
            _appBuilder.Configuration.AddEnvironmentVariables();

            _appBuilder.Services.ConfigureEngineSettings(_appBuilder);


            var niisSettings = Singleton<NI2SSettings>.Instance;
            var useAutofac = niisSettings.Get<CommonConfig>().UseAutofac;

            if (useAutofac)
                _appBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            else
                _appBuilder.UseDefaultServiceProvider(options =>
                {
                    //we don't validate the scopes, since at the app start and the initial configuration we need 
                    //to resolve some services (registered as "scoped") through the root container
                    options.ValidateScopes = false;
                    options.ValidateOnBuild = true;
                });

            //add services to the engine and configure service provider
            _appBuilder.Services.ConfigureEngineServices(_appBuilder);

        }

        //private readonly HostBuilder _hostBuilder;

        //public NiisNodeBuilder(string[] args)
        //{
        //    _hostBuilder = new HostBuilder();

        //    _hostBuilder.ConfigureDefaults(args);

        //    _hostBuilder.ConfigureAppConfiguration(config =>
        //    {
        //        config.AddJsonFile(ConfigurationDefaults.SettingsFilePath, true, true);
        //        if (!string.IsNullOrEmpty(_hostBuilder.Environment?.EnvironmentName))
        //        {
        //            var path = string.Format(ConfigurationDefaults.SettingsEnvironmentFilePath, _hostBuilder.Environment.EnvironmentName);
        //            config.AddJsonFile(path, true, true);
        //        }
        //        config.AddEnvironmentVariables();

        //    });

        //    _hostBuilder.ConfigureServices(services =>
        //    {
        //        services.ConfigureEngineSettings(_hostBuilder);

        //        var niisSettings = Singleton<NI2SSettings>.Instance;
        //        var useAutofac = niisSettings.Get<CommonConfig>().UseAutofac;

        //        if (useAutofac)
        //            _hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        //        else
        //            _hostBuilder.UseDefaultServiceProvider(options =>
        //            {
        //                //we don't validate the scopes, since at the app start and the initial configuration we need 
        //                //to resolve some services (registered as "scoped") through the root container
        //                options.ValidateScopes = false;
        //                options.ValidateOnBuild = true;
        //            });

        //        //add services to the engine and configure service provider
        //        services.ConfigureEngineServices(_hostBuilder);
        //    });

        //}

        internal NiisNode Build() => new NiisNode(_appBuilder.Build());
    }
}