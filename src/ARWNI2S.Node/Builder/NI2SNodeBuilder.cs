using ARWNI2S.Engine.Hosting;
using ARWNI2S.Node.Builder.Extensions;
using ARWNI2S.Node.Hosting;
using ARWNI2S.Node.Hosting.Internals;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace ARWNI2S.Node.Builder
{
    public sealed class NI2SNodeBuilder : IHostApplicationBuilder
    {
        private readonly HostApplicationBuilder _hostApplicationBuilder;
        private readonly ServiceDescriptor _genericNiisNodeServiceDescriptor;
        private NI2SNode _builtNode;



        /// <summary>
        /// A collection of configuration providers for the application to compose. This is useful for adding new configuration sources and providers.
        /// </summary>
        public ConfigurationManager Configuration => _hostApplicationBuilder.Configuration;

        /// <summary>
        /// Provides information about the web hosting environment an application is running.
        /// </summary>
        public INiisHostEnvironment Environment { get; private set; }

        /// <summary>
        /// A collection of logging providers for the application to compose. This is useful for adding new logging providers.
        /// </summary>
        public ILoggingBuilder Logging => _hostApplicationBuilder.Logging;

        /// <summary>
        /// Allows enabling metrics and directing their output.
        /// </summary>
        public IMetricsBuilder Metrics => _hostApplicationBuilder.Metrics;

        /// <summary>
        /// A collection of services for the application to compose. This is useful for adding user provided or framework provided services.
        /// </summary>
        public IServiceCollection Services => _hostApplicationBuilder.Services;

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

        internal NI2SNodeBuilder(NI2SNodeOptions options, Action<IHostBuilder> configureDefaults = null)
        {
            var configuration = new ConfigurationManager();

            configuration.AddEnvironmentVariables(prefix: "NI2S_");

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
                    new KeyValuePair<string, string>(NI2SHostingDefaults.NI2SRootKey, options.NI2SRootPath),
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

            _genericNiisNodeServiceDescriptor = InitializeHosting(bootstrapHostBuilder);
        }

        private void InitializeHostSettings(INiisHostBuilder niisHostBuilder)
        {
            //HACK
            niisHostBuilder.UseSetting(NI2SHostingDefaults.ApplicationKey, _hostApplicationBuilder.Environment.ApplicationName ?? "");
            niisHostBuilder.UseSetting(NI2SHostingDefaults.PreventHostingStartupKey, Configuration[NI2SHostingDefaults.PreventHostingStartupKey]);
            niisHostBuilder.UseSetting(NI2SHostingDefaults.HostingStartupAssembliesKey, Configuration[NI2SHostingDefaults.HostingStartupAssembliesKey]);
            niisHostBuilder.UseSetting(NI2SHostingDefaults.HostingStartupExcludeAssembliesKey, Configuration[NI2SHostingDefaults.HostingStartupExcludeAssembliesKey]);
        }

        [MemberNotNull(nameof(Environment), nameof(Host), nameof(NI2SHost))]
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

            //HACK NI2SSettings = (NI2SSettings)bootstrapHostBuilder.Properties[typeof(NI2SSettings)];

            return genericNI2SHostServiceDescriptor;
        }

        IDictionary<object, object> IHostApplicationBuilder.Properties => ((IHostApplicationBuilder)_hostApplicationBuilder).Properties;

        IConfigurationManager IHostApplicationBuilder.Configuration => Configuration;

        IHostEnvironment IHostApplicationBuilder.Environment => Environment;

        public NI2SNode Build()
        {
            // ConfigureContainer callbacks run after ConfigureServices callbacks including the one that adds GenericNI2SHostService by default.
            // One nice side effect is this gives a way to configure an IHostedService that starts after the server and stops beforehand.
            _hostApplicationBuilder.Services.Add(_genericNiisNodeServiceDescriptor);
            //HACK Host.ApplyServiceProviderFactory(_hostApplicationBuilder);
            _builtNode = new NI2SNode(_hostApplicationBuilder.Build());
            //HACK EngineContext.Current.ConfigureEngine(_builtNodeEngineHost);
            return _builtNode;
        }

        private void ConfigureEngine(NI2SHostBuilderContext context, IEngineBuilder engine) =>
            ConfigureEngine(context, engine, allowDeveloperExceptionPage: true);

        private void ConfigureEngine(NI2SHostBuilderContext context, IEngineBuilder engine, bool allowDeveloperExceptionPage)
        {
            Debug.Assert(_builtNode is not null);


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
