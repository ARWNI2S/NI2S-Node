using ARWNI2S.Configuration;
using ARWNI2S.Engine;
using ARWNI2S.Engine.Builder;
using ARWNI2S.Engine.Configuration;
using ARWNI2S.Engine.Hosting;
using ARWNI2S.Node.Builder;
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
        //private AggregateException _hostingStartupErrors;

        public NI2SHostBuilder(IHostBuilder builder/*, HostBuilderOptions options*/)
        //: base(builder, options)
        {
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
                config.AddEnvironmentVariables(prefix: $"NI2S_{context.HostingEnvironment.ApplicationName.ToUpperInvariant().Replace(".", "_")}_");
            });

            _builder.ConfigureServices((context, services) => //1
            {
                var niisContext = GetNI2SHostBuilderContext(context);
                //var niisHostOptions = (NI2SHostingOptions)context.Properties[typeof(NI2SHostingOptions)];

                // Add the INiisHostEnvironment
                services.AddSingleton(niisContext.HostingEnvironment);

                // REVIEW: This is bad since we don't own this type. Anybody could add one of these and it would mess things up
                // We need to flow this differently
                services.TryAddSingleton(sp => new DiagnosticListener("ARWNI2S"));
                services.TryAddSingleton<DiagnosticSource>(sp => sp.GetRequiredService<DiagnosticListener>());
                services.TryAddSingleton(sp => new ActivitySource("ARWNI2S"));
                services.TryAddSingleton(DistributedContextPropagator.Current);

                //create default file provider
                services.ConfigureFileProvider(niisContext.HostingEnvironment);
                if (!context.Properties.TryGetValue(typeof(NodeSettings), out var settingsVal))
                {
                    context.Properties[typeof(NodeSettings)] = services.ConfigureNodeSettings(context.Configuration);
                }

                // TODO: ApplicationLifetime vs nodeLifecycle

                //HACK
                //services.TryAddSingleton<INiisContextFactory, DefaultContextFactory>();
                //services.TryAddScoped<IMiddlewareFactory, MiddlewareFactory>();
                //services.TryAddSingleton<IEngineBuilderFactory, EngineBuilderFactory>();

                services.AddMetrics();
                //HACK
                //services.TryAddSingleton<HostingMetrics>();

                //HACK
                // IMPORTANT: This needs to run *before* direct calls on the builder (like UseStartup)
                //_hostingStartupHostBuilder?.ConfigureServices(niishostContext, services);

                // Support UseStartup(assemblyName)
                //if (!string.IsNullOrEmpty(niisHostOptions.StartupAssembly))
                //{
                //ScanAssemblyAndRegisterStartup(context, services, niishostContext, niisHostOptions);
                //}

                //services.TryAddSingleton(InitializeNodeEngineSettings(context, niishostContext, services));

            });


        }

        public INiisHostBuilder ConfigureEngine(Action<NI2SHostBuilderContext, IEngineBuilder> configure)
        {
            var startupAssemblyName = configure.GetMethodInfo().DeclaringType!.Assembly.GetName().Name!;

            UseSetting(NI2SHostingDefaults.ApplicationKey, startupAssemblyName);

            _builder.ConfigureServices((context, services) => //6
            {
                //HACK
                //services.AddDefaultContextAccessor();

                ////initialize engine and plugins
                //services.AddNI2SCore().InitializePlugins(context.Configuration);

                //EngineContext.Create().ConfigureServices(services, context.Configuration);

                //HACK
                //services.Configure<NI2SHostServiceOptions>(options =>
                //{
                //    var ni2shostBuilderContext = GetNI2SHostBuilderContext(context);
                //    options.ConfigureEngine = engine => configure(ni2shostBuilderContext, engine);
                //});
            });

            return this;
        }

        //        // This exists just so that we can use ActivatorUtilities.CreateInstance on the Startup class
        //        private sealed class HostServiceProvider : IServiceProvider
        //        {
        //            private readonly NI2SHostBuilderContext _context;

        //            public HostServiceProvider(NI2SHostBuilderContext context)
        //            {
        //                _context = context;
        //            }

        //            public object GetService(Type serviceType)
        //            {
        //                // The implementation of the HostingEnvironment supports both interfaces
        //#pragma warning disable CS0618 // Type or member is obsolete
        //                if (serviceType == typeof(Microsoft.Extensions.Hosting.IHostingEnvironment)
        //                    || serviceType == typeof(IHostingEnvironment)
        //#pragma warning restore CS0618 // Type or member is obsolete
        //                        || serviceType == typeof(INiisHostEnvironment)
        //                    || serviceType == typeof(IHostEnvironment)
        //                    )
        //                {
        //                    return _context.HostingEnvironment;
        //                }

        //                if (serviceType == typeof(IConfiguration))
        //                {
        //                    return _context.Configuration;
        //                }

        //                return null;
        //            }
        //        }


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

        //HACK
        //protected NodeSettings InitializeNodeEngineSettings(HostBuilderContext context, NI2SHostBuilderContext niisContext, IServiceCollection services)
        //{
        //    ////create default file provider
        //    //CommonHelper.DefaultFileProvider ??= new NI2SFileProvider(niisContext.HostingEnvironment);

        //    if (!context.Properties.TryGetValue(typeof(NodeSettings), out var settingsVal))
        //    {
        //        //add configuration parameters
        //        var configurations = services.GetOrCreateTypeFinder()
        //            .FindClassesOfType<IConfig>()
        //            .Select(configType => (IConfig)Activator.CreateInstance(configType))
        //            .ToList();

        //        foreach (var config in configurations)
        //            context.Configuration.GetSection(config.Name).Bind(config, options => options.BindNonPublicProperties = true);

        //        settingsVal = NI2SSettingsHelper.SaveNI2SSettings(configurations, NI2SFileProvider.Default, false);
        //        context.Properties[typeof(NodeSettings)] = settingsVal;
        //    }

        //    var niisSettings = (NodeSettings)settingsVal;
        //    return niisSettings;
        //}

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