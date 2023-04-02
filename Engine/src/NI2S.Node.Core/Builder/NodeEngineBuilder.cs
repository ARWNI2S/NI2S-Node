using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NI2S.Node.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NI2S.Node.Builder
{
    /// <summary>
    /// A builder for node engine and services.
    /// </summary>
    /// <remarks>Was <see cref="NodeEngineBuilder"/></remarks>
    public sealed class NodeEngineBuilder
    {
        private const string EndpointRouteBuilderKey = "__EndpointRouteBuilder";
        private const string AuthenticationMiddlewareSetKey = "__AuthenticationMiddlewareSet";
        private const string AuthorizationMiddlewareSetKey = "__AuthorizationMiddlewareSet";
        private const string UseRoutingKey = "__UseRouting";

        private readonly HostApplicationBuilder _hostEngineBuilder;
        private readonly ServiceDescriptor _genericNodeHostServiceDescriptor;

        private NodeEngine _builtEngine;

        internal NodeEngineBuilder(NodeEngineOptions options, Action<IHostBuilder> configureDefaults = null)
        {
            var configuration = new ConfigurationManager();

            configuration.AddEnvironmentVariables(prefix: "DOTNET_");

            _hostEngineBuilder = new HostApplicationBuilder(new HostApplicationBuilderSettings
            {
                Args = options.Args,
                ApplicationName = options.ApplicationName,
                EnvironmentName = options.EnvironmentName,
                ContentRootPath = options.ContentRootPath,
                Configuration = configuration,
            });

            // Set NodeRootPath if necessary
            if (options.NodeRootPath is not null)
            {
                Configuration.AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>(NodeHostDefaults.NodeRootKey, options.NodeRootPath),
                });
            }

            // Run methods to configure web host defaults early to populate services
            var bootstrapHostBuilder = new BootstrapHostBuilder(_hostEngineBuilder);

            // This is for testing purposes
            configureDefaults?.Invoke(bootstrapHostBuilder);

            bootstrapHostBuilder.ConfigureNodeHostDefaults(nodeHostBuilder =>
            {
                // Runs inline.
                nodeHostBuilder.Configure(ConfigureEngine);

                nodeHostBuilder.UseSetting(NodeHostDefaults.ApplicationKey, _hostEngineBuilder.Environment.ApplicationName ?? "");
                nodeHostBuilder.UseSetting(NodeHostDefaults.PreventHostingStartupKey, Configuration[NodeHostDefaults.PreventHostingStartupKey]);
                nodeHostBuilder.UseSetting(NodeHostDefaults.HostingStartupAssembliesKey, Configuration[NodeHostDefaults.HostingStartupAssembliesKey]);
                nodeHostBuilder.UseSetting(NodeHostDefaults.HostingStartupExcludeAssembliesKey, Configuration[NodeHostDefaults.HostingStartupExcludeAssembliesKey]);
            },
            options =>
            {
                // We've already applied "DOTNET_" environment variables to hosting config
                options.SuppressEnvironmentConfiguration = true;
            });

            // This applies the config from ConfigureNodeHostDefaults
            // Grab the GenericNodeHostService ServiceDescriptor so we can append it after any user-added IHostedServices during Build();
            _genericNodeHostServiceDescriptor = bootstrapHostBuilder.RunDefaultCallbacks();

            // Grab the NodeHostBuilderContext from the property bag to use in the ConfigureNodeHostBuilder. Then
            // grab the INodeHostEnvironment from the nodeHostContext. This also matches the instance in the IServiceCollection.
            var nodeHostContext = (NodeHostBuilderContext)bootstrapHostBuilder.Properties[typeof(NodeHostBuilderContext)];
            Environment = nodeHostContext.HostingEnvironment;

            Host = new ConfigureHostBuilder(bootstrapHostBuilder.Context, Configuration, Services);
            NodeHost = new ConfigureNodeHostBuilder(nodeHostContext, Configuration, Services);
        }

        /// <summary>
        /// Provides information about the web hosting environment an application is running.
        /// </summary>
        public INodeHostEnvironment Environment { get; }

        /// <summary>
        /// A collection of services for the application to compose. This is useful for adding user provided or framework provided services.
        /// </summary>
        public IServiceCollection Services => _hostEngineBuilder.Services;

        /// <summary>
        /// A collection of configuration providers for the application to compose. This is useful for adding new configuration sources and providers.
        /// </summary>
        public ConfigurationManager Configuration => _hostEngineBuilder.Configuration;

        /// <summary>
        /// A collection of logging providers for the application to compose. This is useful for adding new logging providers.
        /// </summary>
        public ILoggingBuilder Logging => _hostEngineBuilder.Logging;

        /// <summary>
        /// An <see cref="INodeHostBuilder"/> for configuring server specific properties, but not building.
        /// To build after configuration, call <see cref="Build"/>.
        /// </summary>
        public ConfigureNodeHostBuilder NodeHost { get; }

        /// <summary>
        /// An <see cref="IHostBuilder"/> for configuring host specific properties, but not building.
        /// To build after configuration, call <see cref="Build"/>.
        /// </summary>
        public ConfigureHostBuilder Host { get; }

        /// <summary>
        /// Builds the <see cref="NodeEngine"/>.
        /// </summary>
        /// <returns>A configured <see cref="NodeEngine"/>.</returns>
        public NodeEngine Build()
        {
            // ConfigureContainer callbacks run after ConfigureServices callbacks including the one that adds GenericNodeHostService by default.
            // One nice side effect is this gives a way to configure an IHostedService that starts after the server and stops beforehand.
            _hostEngineBuilder.Services.Add(_genericNodeHostServiceDescriptor);
            Host.ApplyServiceProviderFactory(_hostEngineBuilder);
            _builtEngine = new NodeEngine(_hostEngineBuilder.Build());
            return _builtEngine;
        }

        private void ConfigureEngine(NodeHostBuilderContext context, IEngineBuilder app)
        {
            Debug.Assert(_builtEngine is not null);

            // UseRouting called before NodeEngine such as in a StartupFilter
            // lets remove the property and reset it at the end so we don't mess with the routes in the filter
            if (app.Properties.TryGetValue(EndpointRouteBuilderKey, out var priorRouteBuilder))
            {
                app.Properties.Remove(EndpointRouteBuilderKey);
            }

            if (context.HostingEnvironment.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }

            // Wrap the entire destination pipeline in UseRouting() and UseEndpoints(), essentially:
            // destination.UseRouting()
            // destination.Run(source)
            // destination.UseEndpoints()

            // Set the route builder so that UseRouting will use the NodeEngine as the IEndpointRouteBuilder for route matching
            app.Properties.Add(NodeEngine.GlobalEndpointRouteBuilderKey, _builtEngine);

            // Only call UseRouting() if there are endpoints configured and UseRouting() wasn't called on the global route builder already
            if (_builtEngine.DataSources.Count > 0)
            {
                // If this is set, someone called UseRouting() when a global route builder was already set
                if (!_builtEngine.Properties.TryGetValue(EndpointRouteBuilderKey, out var localRouteBuilder))
                {
                    //app.UseRouting();
                    // Middleware the needs to re-route will use this property to call UseRouting()
                    _builtEngine.Properties[UseRoutingKey] = app.Properties[UseRoutingKey];
                }
                else
                {
                    // UseEndpoints will be looking for the RouteBuilder so make sure it's set
                    app.Properties[EndpointRouteBuilderKey] = localRouteBuilder;
                }
            }

            // Process authorization and authentication middlewares independently to avoid
            // registering middlewares for services that do not exist
            var serviceProviderIsService = _builtEngine.Services.GetService<IServiceProviderIsService>();
            //if (serviceProviderIsService?.IsService(typeof(IAuthenticationSchemeProvider)) is true)
            //{
            //    // Don't add more than one instance of the middleware
            //    if (!_builtEngine.Properties.ContainsKey(AuthenticationMiddlewareSetKey))
            //    {
            //        // The Use invocations will set the property on the outer pipeline,
            //        // but we want to set it on the inner pipeline as well.
            //        _builtEngine.Properties[AuthenticationMiddlewareSetKey] = true;
            //        app.UseAuthentication();
            //    }
            //}

            //if (serviceProviderIsService?.IsService(typeof(IAuthorizationHandlerProvider)) is true)
            //{
            //    if (!_builtEngine.Properties.ContainsKey(AuthorizationMiddlewareSetKey))
            //    {
            //        _builtEngine.Properties[AuthorizationMiddlewareSetKey] = true;
            //        app.UseAuthorization();
            //    }
            //}

            // Wire the source pipeline to run in the destination pipeline
            app.Use(next =>
            {
                //_builtEngine.Run(next);
                return _builtEngine.BuildRequestDelegate();
            });

            if (_builtEngine.DataSources.Count > 0)
            {
                // We don't know if user code called UseEndpoints(), so we will call it just in case, UseEndpoints() will ignore duplicate DataSources
                //app.UseEndpoints(_ => { });
            }

            // Copy the properties to the destination app builder
            foreach (var item in _builtEngine.Properties)
            {
                app.Properties[item.Key] = item.Value;
            }

            // Remove the route builder to clean up the properties, we're done adding routes to the pipeline
            app.Properties.Remove(NodeEngine.GlobalEndpointRouteBuilderKey);

            // reset route builder if it existed, this is needed for StartupFilters
            if (priorRouteBuilder is not null)
            {
                app.Properties[EndpointRouteBuilderKey] = priorRouteBuilder;
            }
        }
    }
}
