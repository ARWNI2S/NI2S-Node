// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NI2S.Node.Hosting.Builder
{
    /// <summary>
    /// A builder for NI2S node engine hosting and services.
    /// </summary>
    public sealed class NodeEngineHostBuilder
    {
        private readonly HostApplicationBuilder _hostApplicationBuilder;
        private readonly ServiceDescriptor _genericNodeHostServiceDescriptor;

        private NodeEngineHost _builtNodeEngineHost;

        /* 001 - new NodeEngineHostBuilder(...) */
        internal NodeEngineHostBuilder(NodeEngineHostOptions options, Action<IHostBuilder> configureDefaults = null)
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

            // Set NodeRootPath if necessary
            if (options.NodeRootPath is not null)
            {
                Configuration.AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>(NodeHostDefaults.NodeRootKey, options.NodeRootPath),
                });
            }

            // Run methods to configure web host defaults early to populate services
            var bootstrapHostBuilder = new BootstrapHostBuilder(_hostApplicationBuilder);

            // This is for testing purposes
            configureDefaults?.Invoke(bootstrapHostBuilder);

            bootstrapHostBuilder.ConfigureNodeHostDefaults(nodeHostBuilder =>
            {
                /* 001.2.1.3.2 - new NodeEngineHostBuilder(...) -> bootstrapHostBuilder.ConfigureNodeHostDefaults(...) -> builder.ConfigureNodeHost(...)
                                 -> configure(nodehostBuilder) -> configure(nodeHostBuilder) */
                // Runs inline.
                nodeHostBuilder.Configure(ConfigureApplication);

                nodeHostBuilder.UseSetting(NodeHostDefaults.ApplicationKey, _hostApplicationBuilder.Environment.ApplicationName ?? "");
                nodeHostBuilder.UseSetting(NodeHostDefaults.PreventHostingStartupKey, Configuration[NodeHostDefaults.PreventHostingStartupKey]);
                nodeHostBuilder.UseSetting(NodeHostDefaults.HostingStartupAssembliesKey, Configuration[NodeHostDefaults.HostingStartupAssembliesKey]);
                nodeHostBuilder.UseSetting(NodeHostDefaults.HostingStartupExcludeAssembliesKey, Configuration[NodeHostDefaults.HostingStartupExcludeAssembliesKey]);
            },
            options =>
            {
                /* 001.2.1.1 - new NodeEngineHostBuilder(...) -> bootstrapHostBuilder.ConfigureNodeHostDefaults(...)
                               -> builder.ConfigureNodeHost(...) -> configureNodeHostBuilder(nodeHostBuilderOptions) */
                // We've already applied "NI2S_" environment variables to hosting config
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
        /// A collection of services for the application to compose. This is useful for adding user provided or framework provided services.
        /// </summary>
        public ConfigurationManager Configuration => _hostApplicationBuilder.Configuration;

        /// <summary>
        /// A collection of services for the application to compose. This is useful for adding user provided or framework provided services.
        /// </summary>
        public IServiceCollection Services => _hostApplicationBuilder.Services;

        /// <summary>
        /// Provides information about the hosting environment an application is running in.
        /// </summary>
        public INodeHostEnvironment Environment { get; }

        /// <summary>
        /// A collection of logging providers for the application to compose. This is useful for adding new logging providers.
        /// </summary>
        public ILoggingBuilder Logging => _hostApplicationBuilder.Logging;

        /// <summary>
        /// Builds the <see cref="NodeEngineHost"/>.
        /// </summary>
        /// <returns>A configured <see cref="NodeEngineHost"/>.</returns>
        /* 003 - ... -> .Build() */
        public NodeEngineHost Build()
        {
            // ConfigureContainer callbacks run after ConfigureServices callbacks including the one that adds GenericNodeHostService by default.
            // One nice side effect is this gives a way to configure an IHostedService that starts after the server and stops beforehand.
            _hostApplicationBuilder.Services.Add(_genericNodeHostServiceDescriptor);
            Host.ApplyServiceProviderFactory(_hostApplicationBuilder);
            _builtNodeEngineHost = new NodeEngineHost(_hostApplicationBuilder.Build());
            return _builtNodeEngineHost;
        }

        public ConfigureHostBuilder Host { get; }
        public ConfigureNodeHostBuilder NodeHost { get; }

        private void ConfigureApplication(NodeHostBuilderContext context, INodeEngineBuilder engine)
        {
            Debug.Assert(_builtNodeEngineHost is not null);

            // UseRouting called before NodeEngineHost such as in a StartupFilter
            // lets remove the property and reset it at the end so we don't mess with the routes in the filter
            //if (engine.Properties.TryGetValue(EndpointRouteBuilderKey, out var priorRouteBuilder))
            //{
            //    engine.Properties.Remove(EndpointRouteBuilderKey);
            //}

            //if (context.HostingEnvironment.IsDevelopment())
            //{
            //    engine.UseDeveloperExceptionPage();
            //}

            // Wrap the entire destination pipeline in UseRouting() and UseEndpoints(), essentially:
            // destination.UseRouting()
            // destination.Run(source)
            // destination.UseEndpoints()

            // Set the route builder so that UseRouting will use the NodeEngineHost as the IEndpointRouteBuilder for route matching
            engine.Properties.Add(NodeEngineHost.GlobalClusterNodeBuilderKey, _builtNodeEngineHost);

            // Only call UseRouting() if there are endpoints configured and UseRouting() wasn't called on the global route builder already
            //if (_builtNodeEngineHost.DataSources.Count > 0)
            //{
            //    // If this is set, someone called UseRouting() when a global route builder was already set
            //    if (!_builtNodeEngineHost.Properties.TryGetValue(EndpointRouteBuilderKey, out var localRouteBuilder))
            //    {
            //        engine.UseRouting();
            //        // Middleware the needs to re-route will use this property to call UseRouting()
            //        _builtNodeEngineHost.Properties[UseRoutingKey] = engine.Properties[UseRoutingKey];
            //    }
            //    else
            //    {
            //        // UseEndpoints will be looking for the RouteBuilder so make sure it's set
            //        engine.Properties[EndpointRouteBuilderKey] = localRouteBuilder;
            //    }
            //}

            // Process authorization and authentication middlewares independently to avoid
            // registering middlewares for services that do not exist
            var serviceProviderIsService = _builtNodeEngineHost.Services.GetService<IServiceProviderIsService>();
            //if (serviceProviderIsService?.IsService(typeof(IAuthenticationSchemeProvider)) is true)
            //{
            //    // Don't add more than one instance of the middleware
            //    if (!_builtNodeEngineHost.Properties.ContainsKey(AuthenticationMiddlewareSetKey))
            //    {
            //        // The Use invocations will set the property on the outer pipeline,
            //        // but we want to set it on the inner pipeline as well.
            //        _builtNodeEngineHost.Properties[AuthenticationMiddlewareSetKey] = true;
            //        engine.UseAuthentication();
            //    }
            //}

            //if (serviceProviderIsService?.IsService(typeof(IAuthorizationHandlerProvider)) is true)
            //{
            //    if (!_builtNodeEngineHost.Properties.ContainsKey(AuthorizationMiddlewareSetKey))
            //    {
            //        _builtNodeEngineHost.Properties[AuthorizationMiddlewareSetKey] = true;
            //        engine.UseAuthorization();
            //    }
            //}

            // Wire the source pipeline to run in the destination pipeline
            //engine.Use(next =>
            //{
            //    _builtNodeEngineHost.Run(next);
            //    return _builtNodeEngineHost.BuildMessageDelegate();
            //});

            //if (_builtNodeEngineHost.DataSources.Count > 0)
            //{
            //    // We don't know if user code called UseEndpoints(), so we will call it just in case, UseEndpoints() will ignore duplicate DataSources
            //    engine.UseEndpoints(_ => { });
            //}

            // Copy the properties to the destination app builder
            foreach (var item in _builtNodeEngineHost.Properties)
            {
                engine.Properties[item.Key] = item.Value;
            }

            // Remove the route builder to clean up the properties, we're done adding routes to the pipeline
            engine.Properties.Remove(NodeEngineHost.GlobalClusterNodeBuilderKey);

            // reset route builder if it existed, this is needed for StartupFilters
            //if (priorRouteBuilder is not null)
            //{
            //    engine.Properties[EndpointRouteBuilderKey] = priorRouteBuilder;
            //}
        }
    }
}
