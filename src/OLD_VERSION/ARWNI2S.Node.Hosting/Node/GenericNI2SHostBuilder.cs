using ARWNI2S.Hosting.Builder;
using ARWNI2S.Hosting.Configuration;
using ARWNI2S.Hosting.Diagnostics;
using ARWNI2S.Hosting.Internals;

namespace ARWNI2S.Hosting.Node
{
    internal sealed class GenericNI2SHostBuilder : NodeHostBuilderBase
    {
        //private object _startupObject;
        //private readonly object _startupKey = new object();

        private AggregateException _hostingStartupErrors;

        public GenericNI2SHostBuilder(IHostBuilder builder, NI2SHostBuilderOptions options)
            : base(builder, options)
        {
            _builder.ConfigureHostConfiguration(config =>
            {
                config.AddConfiguration(_config);
            });

            // IHostingStartup needs to be executed before any direct methods on the builder
            // so register these callbacks first
            _builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile(NI2SConfigurationDefaults.NI2SSettingsFilePath, true, true);
                if (!string.IsNullOrEmpty(context.HostingEnvironment?.EnvironmentName))
                {
                    var path = string.Format(NI2SConfigurationDefaults.NI2SSettingsEnvironmentFilePath, context.HostingEnvironment.EnvironmentName);
                    config.AddJsonFile(path, true, true);
                }
                config.AddEnvironmentVariables();
            });

            _builder.ConfigureServices((context, services) =>
            {
                var niishostContext = GetNI2SHostBuilderContext(context);
                var niisHostOptions = (NI2SHostOptions)context.Properties[typeof(NI2SHostOptions)];

                // Add the IHostingEnvironment and IApplicationLifetime from Microsoft.AspNetCore.Hosting
                services.AddSingleton(niishostContext.HostingEnvironment);
#pragma warning disable CS0618 // Type or member is obsolete
                services.AddSingleton((IHostingEnvironment)niishostContext.HostingEnvironment);
                //services.AddSingleton<IApplicationLifetime, GenericWebHostApplicationLifetime>();
#pragma warning restore CS0618 // Type or member is obsolete

                services.Configure<NI2SHostServiceOptions>(options =>
                {
                    // Set the options
                    options.NI2SHostOptions = niisHostOptions;
                    // Store and forward any startup errors
                    options.HostingStartupExceptions = _hostingStartupErrors;
                });

                // REVIEW: This is bad since we don't own this type. Anybody could add one of these and it would mess things up
                // We need to flow this differently
                services.TryAddSingleton(sp => new DiagnosticListener("ARWNI2S"));
                services.TryAddSingleton<DiagnosticSource>(sp => sp.GetRequiredService<DiagnosticListener>());
                services.TryAddSingleton(sp => new ActivitySource("ARWNI2S"));
                services.TryAddSingleton(DistributedContextPropagator.Current);

                services.TryAddSingleton<INiisContextFactory, DefaultContextFactory>();
                //services.TryAddScoped<IMiddlewareFactory, MiddlewareFactory>();
                services.TryAddSingleton<IEngineBuilderFactory, EngineBuilderFactory>();

                services.AddMetrics();
                services.TryAddSingleton<HostingMetrics>();

                // IMPORTANT: This needs to run *before* direct calls on the builder (like UseStartup)
                //_hostingStartupHostBuilder?.ConfigureServices(niishostContext, services);

                // Support UseStartup(assemblyName)
                //if (!string.IsNullOrEmpty(niisHostOptions.StartupAssembly))
                //{
                //    ScanAssemblyAndRegisterStartup(context, services, niishostContext, niisHostOptions);
                //}

                services.TryAddSingleton(InitializeNodeEngineSettings(context, niishostContext, services));
            });


        }

        public INiisHostBuilder Configure(Action<NI2SHostBuilderContext, IEngineBuilder> configure)
        {
            var startupAssemblyName = configure.GetMethodInfo().DeclaringType!.Assembly.GetName().Name!;

            UseSetting(NI2SHostDefaults.ApplicationKey, startupAssemblyName);

            _builder.ConfigureServices((context, services) =>
            {
                services.AddDefaultContextAccessor();

                //initialize engine and plugins
                services.AddNI2SCore().InitializePlugins(context.Configuration);

                NI2SContext.Create().ConfigureServices(services, context.Configuration);

                services.Configure<NI2SHostServiceOptions>(options =>
                {
                    var ni2shostBuilderContext = GetNI2SHostBuilderContext(context);
                    options.ConfigureEngine = engine => configure(ni2shostBuilderContext, engine);
                });
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
    }

    //internal class GenericNI2SHostBuilder : INiisHostBuilder
    //{
    //    private IHostBuilder hostBuilder;
    //    private NI2SHostBuilderOptions options;

    //    public GenericNI2SHostBuilder(IHostBuilder hostBuilder, NI2SHostBuilderOptions options)
    //    {
    //        this.hostBuilder = hostBuilder;
    //        this.options = options;
    //    }

    //    public INiisHost Build()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public INiisHostBuilder ConfigureNI2SConfiguration(Action<NI2SHostBuilderContext, IConfigurationBuilder> configureDelegate)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public INiisHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public INiisHostBuilder ConfigureServices(Action<NI2SHostBuilderContext, IServiceCollection> configureServices)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public string GetSetting(string key)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public INiisHostBuilder UseSetting(string key, string value)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}