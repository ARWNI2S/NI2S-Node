using ARWNI2S.Engine.Builder;
using ARWNI2S.Node.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Reflection;

namespace ARWNI2S.Node.Hosting.Internal
{
    internal sealed class NI2SHostBuilder : NodeHostBuilderBase
    {
        //private AggregateException _hostingStartupErrors;

        public NI2SHostBuilder(IHostBuilder builder, HostBuilderOptions options)
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
                //config.AddJsonFile(ConfigDefaults.SettingsFilePath, true, true);
                //if (!string.IsNullOrEmpty(context.HostingEnvironment?.EnvironmentName))
                //{
                //    var path = string.Format(ConfigDefaults.SettingsEnvironmentFilePath, context.HostingEnvironment.EnvironmentName);
                //    config.AddJsonFile(path, true, true);
                //}
                //config.AddEnvironmentVariables();
            });

            _builder.ConfigureServices((context, services) =>
            {
                var niishostContext = GetNI2SHostBuilderContext(context);
                var niisHostOptions = (NI2SHostingOptions)context.Properties[typeof(NI2SHostingOptions)];

                // Add the IHostingEnvironment and IApplicationLifetime from ARWNI2S.Engine.Hosting
                services.AddSingleton(niishostContext.HostingEnvironment);
                //HACK
//#pragma warning disable CS0618 // Type or member is obsolete
//                services.AddSingleton((Engine.Hosting.IHostingEnvironment)niishostContext.HostingEnvironment);
//                services.AddSingleton<IApplicationLifetime, GenericWebHostApplicationLifetime>();
//#pragma warning restore CS0618 // Type or member is obsolete

                //HACK
                //services.Configure<NI2SHostServiceOptions>(options =>
                //{
                //    // Set the options
                //    options.HostingOptions = niisHostOptions;
                //    // Store and forward any startup errors
                //    options.HostingStartupExceptions = _hostingStartupErrors;
                //});

                // REVIEW: This is bad since we don't own this type. Anybody could add one of these and it would mess things up
                // We need to flow this differently
                services.TryAddSingleton(sp => new DiagnosticListener("ARWNI2S"));
                services.TryAddSingleton<DiagnosticSource>(sp => sp.GetRequiredService<DiagnosticListener>());
                services.TryAddSingleton(sp => new ActivitySource("ARWNI2S"));
                services.TryAddSingleton(DistributedContextPropagator.Current);

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
                //    ScanAssemblyAndRegisterStartup(context, services, niishostContext, niisHostOptions);
                //}

                //services.TryAddSingleton(InitializeNodeEngineSettings(context, niishostContext, services));
            });


        }

        public INiisHostBuilder ConfigureEngine(Action<NI2SHostBuilderContext, IEngineBuilder> configure)
        {
            var startupAssemblyName = configure.GetMethodInfo().DeclaringType!.Assembly.GetName().Name!;

            UseSetting(NI2SHostingDefaults.ApplicationKey, startupAssemblyName);

            _builder.ConfigureServices((context, services) =>
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
    }

    //internal class GenericNI2SHostBuilder : INiisHostBuilder
    //{
    //    private IHostBuilder hostBuilder;
    //    private HostBuilderOptions options;

    //    public GenericNI2SHostBuilder(IHostBuilder hostBuilder, HostBuilderOptions options)
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