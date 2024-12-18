using ARWNI2S.Hosting.Configuration;
using ARWNI2S.Hosting.Internals;
using ARWNI2S.Hosting.Node;

namespace ARWNI2S.Hosting.Builder
{
    public sealed class NI2SHostBuilder : IHostApplicationBuilder
    {
        private readonly HostApplicationBuilder _hostApplicationBuilder;
        private readonly ServiceDescriptor _genericNiisHostServiceDescriptor;
        private NI2SHost _builtNodeEngineHost;

        internal NI2SHostBuilder(NI2SNodeOptions options, Action<IHostBuilder> configureDefaults = null)
        {
            var configuration = new ConfigurationManager();

            configuration.AddEnvironmentVariables(prefix: "ARWNI2S_");

            _hostApplicationBuilder = new HostApplicationBuilder(new HostApplicationBuilderSettings
            {
                Args = options.Args,
                ApplicationName = options.ApplicationName,
                EnvironmentName = options.EnvironmentName,
                ContentRootPath = options.ContentRootPath,
                Configuration = configuration,
            });

            // Set NI2SRootPath if necessary
            if (options.NI2SRootPath is not null)
            {
                Configuration.AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>(NI2SHostDefaults.NodeRootKey, options.NI2SRootPath),
                });
            }

            // Run methods to configure web host defaults early to populate services
            var bootstrapHostBuilder = new BootstrapHostBuilder(_hostApplicationBuilder);

            // This is for testing purposes
            configureDefaults?.Invoke(bootstrapHostBuilder);

            bootstrapHostBuilder.ConfigureNI2SHostDefaults(niisHostBuilder =>
            {
                // Runs inline.
                niisHostBuilder.Configure(ConfigureEngine);

                InitializeHostSettings(niisHostBuilder);
            },
            options =>
            {
                // We've already applied "ARWNI2S_" environment variables to hosting config
                options.SuppressEnvironmentConfiguration = true;
            });

            _genericNiisHostServiceDescriptor = InitializeHosting(bootstrapHostBuilder);
        }

        [MemberNotNull(nameof(Environment), nameof(Host), nameof(NI2SHost), nameof(NI2SSettings))]
        private ServiceDescriptor InitializeHosting(BootstrapHostBuilder bootstrapHostBuilder)
        {
            // This applies the config from ConfigureNI2SHostDefaults
            // Grab the GenericNI2SHostService ServiceDescriptor so we can append it after any user-added IHostedServices during Build();
            var genericNI2SHostServiceDescriptor = bootstrapHostBuilder.RunDefaultCallbacks();

            // Grab the NI2SHostBuilderContext from the property bag to use in the ConfigureNI2SHostBuilder. Then
            // grab the INiisHostEnvironment from the niisHostContext. This also matches the instance in the IServiceCollection.
            var niisHostContext = (NI2SHostBuilderContext)bootstrapHostBuilder.Properties[typeof(NI2SHostBuilderContext)];
            Environment = niisHostContext.HostingEnvironment;

            Host = new ConfigureHostBuilder(bootstrapHostBuilder.Context, Configuration, Services);
            NI2SHost = new ConfigureNI2SHostBuilder(niisHostContext, Configuration, Services);

            NI2SSettings = (NI2SSettings)bootstrapHostBuilder.Properties[typeof(NI2SSettings)];

            return genericNI2SHostServiceDescriptor;
        }

        private void InitializeHostSettings(INiisHostBuilder niisHostBuilder)
        {
            niisHostBuilder.UseSetting(NI2SHostDefaults.ApplicationKey, _hostApplicationBuilder.Environment.ApplicationName ?? "");
            niisHostBuilder.UseSetting(NI2SHostDefaults.PreventHostingStartupKey, Configuration[NI2SHostDefaults.PreventHostingStartupKey]);
            niisHostBuilder.UseSetting(NI2SHostDefaults.HostingStartupAssembliesKey, Configuration[NI2SHostDefaults.HostingStartupAssembliesKey]);
            niisHostBuilder.UseSetting(NI2SHostDefaults.HostingStartupExcludeAssembliesKey, Configuration[NI2SHostDefaults.HostingStartupExcludeAssembliesKey]);
        }

        //private static DefaultServiceProviderFactory GetServiceProviderFactory(HostApplicationBuilder hostApplicationBuilder)
        //{
        //    if (hostApplicationBuilder.Environment.IsDevelopment())
        //    {
        //        return new DefaultServiceProviderFactory(
        //            new ServiceProviderOptions
        //            {
        //                ValidateScopes = true,
        //                ValidateOnBuild = true,
        //            });
        //    }

        //    return new DefaultServiceProviderFactory();
        //}

        //private static void SetDefaultContentRoot(NI2SNodeOptions options, ConfigurationManager configuration)
        //{
        //    if (options.ContentRootPath is null && configuration[HostDefaults.ContentRootKey] is null)
        //    {
        //        // Logic taken from https://github.com/dotnet/runtime/blob/dc5a6c8be1644915c14c4a464447b0d54e223a46/src/libraries/Microsoft.Extensions.Hosting/src/HostingHostBuilderExtensions.cs#L209-L227

        //        // If we're running anywhere other than C:\Windows\system32, we default to using the CWD for the ContentRoot.
        //        // However, since many things like Windows services and MSIX installers have C:\Windows\system32 as their CWD, which is not likely
        //        // to be the home for things like appsettings.json, we skip changing the ContentRoot in that case. The non-"default" initial
        //        // value for ContentRoot is AppContext.BaseDirectory (e.g. the executable path) which probably makes more sense than system32.

        //        // In my testing, both Environment.CurrentDirectory and Environment.SystemDirectory return the path without
        //        // any trailing directory separator characters. I'm not even sure the casing can ever be different from these APIs, but I think it makes sense to
        //        // ignore case for Windows path comparisons given the file system is usually (always?) going to be case insensitive for the system path.
        //        string cwd = System.Environment.CurrentDirectory;
        //        if (!OperatingSystem.IsWindows() || !string.Equals(cwd, System.Environment.SystemDirectory, StringComparison.OrdinalIgnoreCase))
        //        {
        //            configuration.AddInMemoryCollection(new[]
        //            {
        //            new KeyValuePair<string, string>(HostDefaults.ContentRootKey, cwd),
        //        });
        //        }
        //    }
        //}

        //private static void ApplyDefaultAppConfigurationSlim(IHostEnvironment env, ConfigurationManager configuration, string[] args)
        //{
        //    // Logic taken from https://github.com/dotnet/runtime/blob/6149ca07d2202c2d0d518e10568c0d0dd3473576/src/libraries/Microsoft.Extensions.Hosting/src/HostingHostBuilderExtensions.cs#L229-L256

        //    var reloadOnChange = GetReloadConfigOnChangeValue(configuration);

        //    configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: reloadOnChange)
        //        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: reloadOnChange);

        //    if (env.IsDevelopment() && env.ApplicationName is { Length: > 0 })
        //    {
        //        try
        //        {
        //            var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
        //            configuration.AddUserSecrets(appAssembly, optional: true, reloadOnChange: reloadOnChange);
        //        }
        //        catch (FileNotFoundException)
        //        {
        //            // The assembly cannot be found, so just skip it.
        //        }
        //    }

        //    configuration.AddEnvironmentVariables();

        //    if (args is { Length: > 0 })
        //    {
        //        configuration.AddCommandLine(args);
        //    }

        //    static bool GetReloadConfigOnChangeValue(ConfigurationManager configuration)
        //    {
        //        const string reloadConfigOnChangeKey = "hostBuilder:reloadConfigOnChange";
        //        var result = true;
        //        if (configuration[reloadConfigOnChangeKey] is string reloadConfigOnChange)
        //        {
        //            if (!bool.TryParse(reloadConfigOnChange, out result))
        //            {
        //                throw new InvalidOperationException($"Failed to convert configuration value at '{configuration.GetSection(reloadConfigOnChangeKey).Path}' to type '{typeof(bool)}'.");
        //            }
        //        }
        //        return result;
        //    }
        //}

        //private static void AddDefaultServicesSlim(ConfigurationManager configuration, IServiceCollection services)
        //{
        //    // Add the necessary services for the slim NI2SHostBuilder, taken from https://github.com/dotnet/runtime/blob/6149ca07d2202c2d0d518e10568c0d0dd3473576/src/libraries/Microsoft.Extensions.Hosting/src/HostingHostBuilderExtensions.cs#L266
        //    services.AddLogging(logging =>
        //    {
        //        logging.AddConfiguration(configuration.GetSection("Logging"));
        //        logging.AddSimpleConsole();

        //        logging.Configure(options =>
        //        {
        //            options.ActivityTrackingOptions =
        //                ActivityTrackingOptions.SpanId |
        //                ActivityTrackingOptions.TraceId |
        //                ActivityTrackingOptions.ParentId;
        //        });
        //    });
        //}

        /// <summary>
        /// Provides information about the web hosting environment an application is running.
        /// </summary>
        public INiisHostEnvironment Environment { get; private set; }

        /// <summary>
        /// A collection of services for the application to compose. This is useful for adding user provided or framework provided services.
        /// </summary>
        public IServiceCollection Services => _hostApplicationBuilder.Services;

        /// <summary>
        /// A collection of configuration providers for the application to compose. This is useful for adding new configuration sources and providers.
        /// </summary>
        public ConfigurationManager Configuration => _hostApplicationBuilder.Configuration;

        /// <summary>
        /// A collection of logging providers for the application to compose. This is useful for adding new logging providers.
        /// </summary>
        public ILoggingBuilder Logging => _hostApplicationBuilder.Logging;

        /// <summary>
        /// Allows enabling metrics and directing their output.
        /// </summary>
        public IMetricsBuilder Metrics => _hostApplicationBuilder.Metrics;

        /// <summary>
        /// An <see cref="INiisHostBuilder"/> for configuring server specific properties, but not building.
        /// To build after configuration, call <see cref="Build"/>.
        /// </summary>
        public ConfigureNI2SHostBuilder NI2SHost { get; private set; }

        /// <summary>
        /// An <see cref="IHostBuilder"/> for configuring host specific properties, but not building.
        /// To build after configuration, call <see cref="Build"/>.
        /// </summary>
        public ConfigureHostBuilder Host { get; private set; }
        
        public NI2SSettings NI2SSettings { get; private set; }

        IDictionary<object, object> IHostApplicationBuilder.Properties => ((IHostApplicationBuilder)_hostApplicationBuilder).Properties;

        IConfigurationManager IHostApplicationBuilder.Configuration => Configuration;

        IHostEnvironment IHostApplicationBuilder.Environment => Environment;

        public NI2SHost Build()
        {
            // ConfigureContainer callbacks run after ConfigureServices callbacks including the one that adds GenericNI2SHostService by default.
            // One nice side effect is this gives a way to configure an IHostedService that starts after the server and stops beforehand.
            _hostApplicationBuilder.Services.Add(_genericNiisHostServiceDescriptor);
            Host.ApplyServiceProviderFactory(_hostApplicationBuilder);
            _builtNodeEngineHost = new NI2SHost(_hostApplicationBuilder.Build());
            NI2SContext.Current.ConfigureEngine(_builtNodeEngineHost);
            return _builtNodeEngineHost;
        }

        private void ConfigureEngine(NI2SHostBuilderContext context, IEngineBuilder engine) =>
            ConfigureEngine(context, engine, allowDeveloperExceptionPage: true);

        private void ConfigureEngine(NI2SHostBuilderContext context, IEngineBuilder engine, bool allowDeveloperExceptionPage)
        {
            Debug.Assert(_builtNodeEngineHost is not null);


            //// UseRouting called before WebApplication such as in a StartupFilter
            //// lets remove the property and reset it at the end so we don't mess with the routes in the filter
            //if (app.Properties.TryGetValue(EndpointRouteBuilderKey, out var priorRouteBuilder))
            //{
            //    app.Properties.Remove(EndpointRouteBuilderKey);
            //}

            //if (allowDeveloperExceptionPage && context.HostingEnvironment.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //// Wrap the entire destination pipeline in UseRouting() and UseEndpoints(), essentially:
            //// destination.UseRouting()
            //// destination.Run(source)
            //// destination.UseEndpoints()

            //// Set the route builder so that UseRouting will use the WebApplication as the IClusterNodeBuilder for route matching
            //app.Properties.Add(WebApplication.GlobalEndpointRouteBuilderKey, _builtApplication);

            //// Only call UseRouting() if there are endpoints configured and UseRouting() wasn't called on the global route builder already
            //if (_builtApplication.DataSources.Count > 0)
            //{
            //    // If this is set, someone called UseRouting() when a global route builder was already set
            //    if (!_builtApplication.Properties.TryGetValue(EndpointRouteBuilderKey, out var localRouteBuilder))
            //    {
            //        app.UseRouting();
            //        // Middleware the needs to re-route will use this property to call UseRouting()
            //        _builtApplication.Properties[UseRoutingKey] = app.Properties[UseRoutingKey];
            //    }
            //    else
            //    {
            //        // UseEndpoints will be looking for the RouteBuilder so make sure it's set
            //        app.Properties[EndpointRouteBuilderKey] = localRouteBuilder;
            //    }
            //}

            //// Process authorization and authentication middlewares independently to avoid
            //// registering middlewares for services that do not exist
            //var serviceProviderIsService = _builtApplication.Services.GetService<IServiceProviderIsService>();
            //if (serviceProviderIsService?.IsService(typeof(IAuthenticationSchemeProvider)) is true)
            //{
            //    // Don't add more than one instance of the middleware
            //    if (!_builtApplication.Properties.ContainsKey(AuthenticationMiddlewareSetKey))
            //    {
            //        // The Use invocations will set the property on the outer pipeline,
            //        // but we want to set it on the inner pipeline as well.
            //        _builtApplication.Properties[AuthenticationMiddlewareSetKey] = true;
            //        app.UseAuthentication();
            //    }
            //}

            //if (serviceProviderIsService?.IsService(typeof(IAuthorizationHandlerProvider)) is true)
            //{
            //    if (!_builtApplication.Properties.ContainsKey(AuthorizationMiddlewareSetKey))
            //    {
            //        _builtApplication.Properties[AuthorizationMiddlewareSetKey] = true;
            //        app.UseAuthorization();
            //    }
            //}

            //// Wire the source pipeline to run in the destination pipeline
            //var wireSourcePipeline = new WireSourcePipeline(_builtApplication);
            //app.Use(wireSourcePipeline.CreateMiddleware);

            //if (_builtApplication.DataSources.Count > 0)
            //{
            //    // We don't know if user code called UseEndpoints(), so we will call it just in case, UseEndpoints() will ignore duplicate DataSources
            //    app.UseEndpoints(_ => { });
            //}

            //MergeMiddlewareDescriptions(app);

            //// Copy the properties to the destination app builder
            //foreach (var item in _builtApplication.Properties)
            //{
            //    app.Properties[item.Key] = item.Value;
            //}

            //// Remove the route builder to clean up the properties, we're done adding routes to the pipeline
            //app.Properties.Remove(WebApplication.GlobalEndpointRouteBuilderKey);

            //// Reset route builder if it existed, this is needed for StartupFilters
            //if (priorRouteBuilder is not null)
            //{
            //    app.Properties[EndpointRouteBuilderKey] = priorRouteBuilder;
            //}
        }

        void IHostApplicationBuilder.ConfigureContainer<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory, Action<TContainerBuilder> configure) =>
            _hostApplicationBuilder.ConfigureContainer(factory, configure);
    }
}
