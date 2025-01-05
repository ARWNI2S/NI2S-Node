using ARWNI2S.Configuration;
using ARWNI2S.Engine;
using ARWNI2S.Engine.Builder;
using ARWNI2S.Engine.Hosting;
using ARWNI2S.Hosting;
using ARWNI2S.Hosting.Builder;
using ARWNI2S.Node.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Reflection;

namespace ARWNI2S.Node.Hosting.Internal
{
    internal sealed class NI2SHostBuilder : INiisHostBuilder
    {
        private readonly IHostBuilder _builder;
        private readonly IConfiguration _config;
        private readonly IEngineContext _engineContext;
        private AggregateException _hostingStartupErrors;

        public NI2SHostBuilder(IHostBuilder builder)
        {
            _engineContext = EngineContext.Create();

            _builder = builder;
            var configBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection();

            configBuilder.AddEnvironmentVariables(prefix: "NI2S_");

            _config = configBuilder.Build();

            _builder.ConfigureHostConfiguration(config =>//1
            {
                config.AddConfiguration(_config);
            });

            // IHostingStartup needs to be executed before any direct methods on the builder
            // so register these callbacks first
            _builder.ConfigureAppConfiguration((context, config) =>//1
            {
                config.AddJsonFile(ConfigDefaults.SettingsFilePath, true, true);
                if (!string.IsNullOrEmpty(context.HostingEnvironment?.EnvironmentName))
                {
                    var path = string.Format(ConfigDefaults.SettingsEnvironmentFilePath, context.HostingEnvironment.EnvironmentName);
                    config.AddJsonFile(path, true, true);
                }
            });

            _builder.ConfigureServices((context, services) => //1
            {
                var niisContext = GetNI2SHostBuilderContext(context);
                var niisHostOptions = (NI2SHostingOptions)context.Properties[typeof(NI2SHostingOptions)];

                // Add the INiisHostEnvironment
                services.AddSingleton(niisContext.HostingEnvironment);

                //TODO: Lifetime-Lifecycle review

                services.Configure<NI2SHostServiceOptions>(options =>
                {
                    // Set the options
                    options.HostingOptions = niisHostOptions;
                    // Store and forward any startup errors
                    options.HostingStartupExceptions = _hostingStartupErrors;
                });

                // REVIEW: This is bad since we don't own this type. Anybody could add one of these and it would mess things up
                // We need to flow this differently
                services.TryAddSingleton(sp => new DiagnosticListener("ARWNI2S"));
                services.TryAddSingleton<DiagnosticSource>(sp => sp.GetRequiredService<DiagnosticListener>());
                services.TryAddSingleton(sp => new ActivitySource("ARWNI2S"));
                services.TryAddSingleton(DistributedContextPropagator.Current);

                //create default file provider
                services.ConfigureFileProvider(niisContext.HostingEnvironment);

                //configure and bind setings
                if (!context.Properties.TryGetValue(typeof(NI2SSettings), out var settingsVal))
                {
                    settingsVal = services.BindNodeSettings(context.Configuration);
                }
                context.Properties[typeof(NI2SSettings)] = (NI2SSettings)settingsVal;

                services.TryAddSingleton<INiisContextFactory, EngineContextFactory>();
                services.TryAddScoped<INiisProcessorFactory, ProcessorFactory>();
                services.TryAddSingleton<IEngineBuilderFactory, EngineBuilderFactory>();

                services.AddMetrics();
                services.TryAddSingleton<HostingMetrics>();
            });

        }

        internal INiisHostBuilder ConfigureEngine(Action<NI2SHostBuilderContext, IEngineBuilder> configure)
        {
            var startupAssemblyName = configure.GetMethodInfo().DeclaringType!.Assembly.GetName().Name!;

            UseSetting(NI2SHostingDefaults.ApplicationKey, startupAssemblyName);

            _builder.ConfigureServices((context, services) => //6
            {
                _engineContext.ConfigureServices(services, context.Configuration);

                services.Configure<NI2SHostServiceOptions>(options =>
                {
                    var niishostBuilderContext = GetNI2SHostBuilderContext(context);
                    options.ConfigureEngine = engine => configure(niishostBuilderContext, engine);
                });

            });

            return this;
        }

        public INiisHost Build()
        {
            throw new NotSupportedException($"Building this implementation of {nameof(INiisHostBuilder)} is not supported.");
        }

        public INiisHostBuilder ConfigureNI2SConfiguration(Action<NI2SHostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            _builder.ConfigureAppConfiguration((context, builder) =>
            {
                var niishostBuilderContext = GetNI2SHostBuilderContext(context);
                configureDelegate(niishostBuilderContext, builder);
            });

            return this;
        }

        public INiisHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            return ConfigureServices((context, services) => configureServices(services));
        }

        public INiisHostBuilder ConfigureServices(Action<NI2SHostBuilderContext, IServiceCollection> configureServices)
        {
            _builder.ConfigureServices((context, builder) =>
            {
                var niishostBuilderContext = GetNI2SHostBuilderContext(context);
                configureServices(niishostBuilderContext, builder);
            });

            return this;
        }

        public INiisHostBuilder UseDefaultServiceProvider(Action<NI2SHostBuilderContext, ServiceProviderOptions> configure)
        {
            _builder.UseServiceProviderFactory(context =>
            {
                var niisHostBuilderContext = GetNI2SHostBuilderContext(context);
                var options = new ServiceProviderOptions();
                configure(niisHostBuilderContext, options);
                return new DefaultServiceProviderFactory(options);
            });

            return this;
        }

        protected NI2SHostBuilderContext GetNI2SHostBuilderContext(HostBuilderContext context)
        {
            if (!context.Properties.TryGetValue(typeof(NI2SHostBuilderContext), out var contextVal))
            {
                // Use _config as a fallback for NI2SHostingOptions in case the chained source was removed from the hosting IConfigurationBuilder.
                var options = new NI2SHostingOptions(context.Configuration, fallbackConfiguration: _config, environment: context.HostingEnvironment);
                var niisHostBuilderContext = new NI2SHostBuilderContext
                {
                    Configuration = context.Configuration,
                    HostingEnvironment = new HostingEnvironment(),
                    EngineContext = _engineContext
                };
                niisHostBuilderContext.HostingEnvironment.Initialize(context.HostingEnvironment.ContentRootPath, options, baseEnvironment: context.HostingEnvironment);
                context.Properties[typeof(NI2SHostBuilderContext)] = niisHostBuilderContext;
                context.Properties[typeof(NI2SHostingOptions)] = options;
                return niisHostBuilderContext;
            }

            // Refresh config, it's periodically updated/replaced
            var niisHostContext = (NI2SHostBuilderContext)contextVal;
            niisHostContext.Configuration = context.Configuration;
            return niisHostContext;
        }

        public string GetSetting(string key)
        {
            return _config[key];
        }

        public INiisHostBuilder UseSetting(string key, string value)
        {
            _config[key] = value;
            return this;
        }
    }
}