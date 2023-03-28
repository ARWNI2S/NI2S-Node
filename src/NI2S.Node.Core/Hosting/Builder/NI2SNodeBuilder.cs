using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NI2S.Node.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NI2S.Node.Hosting.Builder
{
    /// <summary>
    /// A builder for NI2S node and services.
    /// </summary>
    public sealed class NI2SNodeBuilder
    {
        //TODO: Rewrite constants to really usable ones
        private const string EndpointRouteBuilderKey = "__EndpointRouteBuilder";
        private const string AuthenticationMiddlewareSetKey = "__AuthenticationMiddlewareSet";
        private const string AuthorizationMiddlewareSetKey = "__AuthorizationMiddlewareSet";
        private const string UseRoutingKey = "__UseRouting";

        private readonly HostApplicationBuilder _hostApplicationBuilder;
        private readonly ServiceDescriptor _genericNodeHostServiceDescriptor;

        private NI2SNode _builtNI2SNode;

        #region Properties

        /// <summary>
        /// Provides information about the web hosting environment an application is running.
        /// </summary>
        public INodeHostEnvironment Environment { get; }

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
        /// An <see cref="INodeHostBuilder"/> for configuring server specific properties, but not building.
        /// To build after configuration, call <see cref="Build"/>.
        /// </summary>
        public ConfigureNodeHostBuilder NodeHost { get; }

        /// <summary>
        /// An <see cref="IHostBuilder"/> for configuring host specific properties, but not building.
        /// To build after configuration, call <see cref="Build"/>.
        /// </summary>
        public ConfigureHostBuilder Host { get; }

        #endregion

        internal NI2SNodeBuilder(NI2SNodeOptions options, Action<IHostBuilder> configureDefaults = null)
        {
            var configuration = new ConfigurationManager();
            configuration.AddEnvironmentVariables(prefix: "NETCORE_");

            _hostApplicationBuilder = new HostApplicationBuilder(new HostApplicationBuilderSettings
            {
                Args = options.Args,
                ApplicationName = $"NI2S_Node_{options.NodeGuid}",
                EnvironmentName = options.EnvironmentName,
                ContentRootPath = options.ContentRootPath,
                Configuration = configuration,
            });

            // Set AssetsRootPath if necessary
            if (options.AssetsRootPath is not null)
            {
                Configuration.AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>(NodeHostDefaults.AssetsRootKey, options.AssetsRootPath),
                });
            }

            //TODO: Check BootstrapHostBuilder
            // Run methods to configure web host defaults early to populate services
            var bootstrapHostBuilder = new BootstrapHostBuilder(_hostApplicationBuilder);

            // This is for testing purposes
            configureDefaults?.Invoke(bootstrapHostBuilder);

            //TODO: Check GenericHostBuilderExtensions
            bootstrapHostBuilder.ConfigureNodeHostDefaults(nodeHostBuilder =>
            {
                // Runs inline.
                nodeHostBuilder.Configure(ConfigureNode);

                nodeHostBuilder.UseSetting(NodeHostDefaults.ApplicationKey, _hostApplicationBuilder.Environment.ApplicationName ?? "");
                nodeHostBuilder.UseSetting(NodeHostDefaults.PreventHostingStartupKey, Configuration[NodeHostDefaults.PreventHostingStartupKey]);
                nodeHostBuilder.UseSetting(NodeHostDefaults.HostingStartupAssembliesKey, Configuration[NodeHostDefaults.HostingStartupAssembliesKey]);
                nodeHostBuilder.UseSetting(NodeHostDefaults.HostingStartupExcludeAssembliesKey, Configuration[NodeHostDefaults.HostingStartupExcludeAssembliesKey]);
            },
            options =>
            {
                // We've already applied "NETCORE_" environment variables to hosting config
                options.SuppressEnvironmentConfiguration = true;
            });

            // This applies the config from ConfigureNodeHostDefaults
            // Grab the GenericNodeHostService ServiceDescriptor so we can append it after any user-added IHostedServices during Build();
            _genericNodeHostServiceDescriptor = bootstrapHostBuilder.RunDefaultCallbacks();

            // Grab the NodeHostBuilderContext from the property bag to use in the ConfigureNodeHostBuilder. Then
            // grab the INodeHostEnvironment from the webHostContext. This also matches the instance in the IServiceCollection.
            var nodeHostContext = (NodeHostBuilderContext)bootstrapHostBuilder.Properties[typeof(NodeHostBuilderContext)];
            Environment = nodeHostContext.HostingEnvironment;

            Host = new ConfigureHostBuilder(bootstrapHostBuilder.Context, Configuration, Services);
            NodeHost = new ConfigureNodeHostBuilder(nodeHostContext, Configuration, Services);
        }

        /// <summary>
        /// Builds the <see cref="NI2SNode"/>.
        /// </summary>
        /// <returns>A configured <see cref="NI2SNode"/>.</returns>
        public NI2SNode Build()
        {
            // ConfigureContainer callbacks run after ConfigureServices callbacks including the one that adds GenericNodeHostService by default.
            // One nice side effect is this gives a way to configure an IHostedService that starts after the server and stops beforehand.
            _hostApplicationBuilder.Services.Add(_genericNodeHostServiceDescriptor);
            Host.ApplyServiceProviderFactory(_hostApplicationBuilder);
            _builtNI2SNode = new NI2SNode(_hostApplicationBuilder.Build());
            return _builtNI2SNode;
        }

        private void ConfigureNode(NodeHostBuilderContext context, INodeBuilder node)
        {
            Debug.Assert(_builtNI2SNode is not null);

            // UseRouting called before WebApplication such as in a StartupFilter
            // lets remove the property and reset it at the end so we don't mess with the routes in the filter
            if (node.Properties.TryGetValue(EndpointRouteBuilderKey, out var priorRouteBuilder))
            {
                node.Properties.Remove(EndpointRouteBuilderKey);
            }

            if (context.HostingEnvironment.IsDevelopment())
            {
                //TODO: IsDevelopment
                //node.UseDeveloperExceptionPage();
            }

            // Wrap the entire destination pipeline in UseRouting() and UseEndpoints(), essentially:
            //destination.UseRouting()
            // destination.Run(source)
            // destination.UseEndpoints()

            // Set the route builder so that UseRouting will use the WebApplication as the IEndpointRouteBuilder for route matching
            node.Properties.Add(NI2SNode.GlobalNodeClusterBuilderKey, _builtNI2SNode);

            // Only call UseRouting() if there are endpoints configured and UseRouting() wasn't called on the global route builder already
            if (_builtNI2SNode.DataSources.Count > 0)
            {
                // If this is set, someone called UseRouting() when a global route builder was already set
                if (!_builtNI2SNode.Properties.TryGetValue(EndpointRouteBuilderKey, out var localRouteBuilder))
                {
                    //node.UseRouting();
                    // Middleware the needs to re-route will use this property to call UseRouting()
                    _builtNI2SNode.Properties[UseRoutingKey] = node.Properties[UseRoutingKey];
                }
                else
                {
                    // UseEndpoints will be looking for the RouteBuilder so make sure it's set
                    node.Properties[EndpointRouteBuilderKey] = localRouteBuilder;
                }
            }

            // Process authorization and authentication middlewares independently to avoid
            // registering middlewares for services that do not exist
            var serviceProviderIsService = _builtNI2SNode.Services.GetService<IServiceProviderIsService>();
            // TODO: CHECK NEXT ALL
            //if (serviceProviderIsService?.IsService(typeof(IAuthenticationSchemeProvider)) is true)
            //{
            //    // Don't add more than one instance of the middleware
            //    if (!_builtNI2SNode.Properties.ContainsKey(AuthenticationMiddlewareSetKey))
            //    {
            //        // The Use invocations will set the property on the outer pipeline,
            //        // but we want to set it on the inner pipeline as well.
            //        _builtNI2SNode.Properties[AuthenticationMiddlewareSetKey] = true;
            //        node.UseAuthentication();
            //    }
            //}

            // TODO: CHECK NEXT ALL
            //if (serviceProviderIsService?.IsService(typeof(IAuthorizationHandlerProvider)) is true)
            //{
            //    if (!_builtNI2SNode.Properties.ContainsKey(AuthorizationMiddlewareSetKey))
            //    {
            //        _builtNI2SNode.Properties[AuthorizationMiddlewareSetKey] = true;
            //        node.UseAuthorization();
            //    }
            //}

            // Wire the source pipeline to run in the destination pipeline
            node.Use(next =>
            {
                _builtNI2SNode.Run(next);
                return _builtNI2SNode.BuildMessageDelegate();
            });

            if (_builtNI2SNode.DataSources.Count > 0)
            {
                // TODO: NODE PORTS
                // We don't know if user code called UseEndpoints(), so we will call it just in case, UseEndpoints() will ignore duplicate DataSources
                //node.UseEndpoints(_ => { });
            }

            // Copy the properties to the destination node builder
            foreach (var item in _builtNI2SNode.Properties)
            {
                node.Properties[item.Key] = item.Value;
            }

            // Remove the route builder to clean up the properties, we're done adding routes to the pipeline
            node.Properties.Remove(NI2SNode.GlobalNodeClusterBuilderKey);

            // reset route builder if it existed, this is needed for StartupFilters
            if (priorRouteBuilder is not null)
            {
                node.Properties[EndpointRouteBuilderKey] = priorRouteBuilder;
            }
        }

    }
}
