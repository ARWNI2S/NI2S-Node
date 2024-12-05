﻿using ARWNI2S.Engine.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using ARWNI2S.Node.Hosting.Internal;
using ARWNI2S.Node.Hosting;
using ARWNI2S.Node.Configuration.Options;
using ARWNI2S.Node.Hosting.Extensions;
using ARWNI2S.Node.Hosting.Generic;

namespace ARWNI2S.Node.Builder
{
    public sealed class NodeEngineBuilder : IHostApplicationBuilder
    {
        //private const string EndpointRouteBuilderKey = "__EndpointRouteBuilder";
        //private const string AuthenticationFrameProcessorSetKey = "__AuthenticationFrameProcessorSet";
        //private const string AuthorizationFrameProcessorSetKey = "__AuthorizationFrameProcessorSet";
        //private const string UseRoutingKey = "__UseRouting";

        private readonly HostApplicationBuilder _hostApplicationBuilder;
        private readonly ServiceDescriptor _genericNodeHostServiceDescriptor;

        private NodeEngine _builtEngine;

        internal NodeEngineBuilder(NodeEngineOptions options, Action<IHostBuilder> configureDefaults = null)
        {
            var configuration = new ConfigurationManager();

            configuration.AddEnvironmentVariables(prefix: "ARWNI2S_");

            _hostApplicationBuilder = new HostApplicationBuilder(new HostApplicationBuilderSettings
            {
                Args = options.Args,
                ApplicationName = options.EngineName,
                EnvironmentName = options.EnvironmentName,
                ContentRootPath = options.ContentRootPath,
                Configuration = configuration,
            });

            // Set NodeRootPath if necessary
            if (options.NodeRootPath is not null)
            {
                Configuration.AddInMemoryCollection([new KeyValuePair<string, string>(NodeHostDefaults.NodeRootKey, options.NodeRootPath)]);
            }

            // Run methods to configure node host defaults early to populate services
            var bootstrapHostBuilder = new BootstrapHostBuilder(_hostApplicationBuilder);

            // This is for testing purposes
            configureDefaults?.Invoke(bootstrapHostBuilder);

            bootstrapHostBuilder.ConfigureNodeHostDefaults(nodeHostBuilder =>
            {
                // Runs inline.
                nodeHostBuilder.Configure(ConfigureEngine);

                InitializeNodeHostSettings(nodeHostBuilder);
            },
            options =>
            {
                // We've already applied "ARWNI2S_" environment variables to hosting config
                options.SuppressEnvironmentConfiguration = true;
            });

            _genericNodeHostServiceDescriptor = InitializeHosting(bootstrapHostBuilder);
        }

        [MemberNotNull(nameof(Environment), nameof(Host), nameof(NodeHost))]
        private ServiceDescriptor InitializeHosting(BootstrapHostBuilder bootstrapHostBuilder)
        {
            // This applies the config from ConfigureNodeHostDefaults
            // Grab the GenericNodeHostService ServiceDescriptor so we can append it after any user-added IHostedServices during Build();
            var genericNodeHostServiceDescriptor = bootstrapHostBuilder.RunDefaultCallbacks();

            // Grab the NodeHostBuilderContext from the property bag to use in the ConfigureNodeHostBuilder. Then
            // grab the IHostEnvironment from the nodeHostContext. This also matches the instance in the IServiceCollection.
            var nodeHostContext = (NodeHostBuilderContext)bootstrapHostBuilder.Properties[typeof(NodeHostBuilderContext)];
            Environment = nodeHostContext.HostingEnvironment;

            Host = new ConfigureHostBuilder(bootstrapHostBuilder.Context, Configuration, Services);
            NodeHost = new ConfigureNodeHostBuilder(nodeHostContext, Configuration, Services);

            return genericNodeHostServiceDescriptor;
        }

        private void InitializeNodeHostSettings(INodeHostBuilder nodeHostBuilder)
        {
            nodeHostBuilder.UseSetting(NodeHostDefaults.ApplicationKey, _hostApplicationBuilder.Environment.ApplicationName ?? "");
            nodeHostBuilder.UseSetting(NodeHostDefaults.PreventHostingStartupKey, Configuration[NodeHostDefaults.PreventHostingStartupKey]);
            nodeHostBuilder.UseSetting(NodeHostDefaults.HostingStartupAssembliesKey, Configuration[NodeHostDefaults.HostingStartupAssembliesKey]);
            nodeHostBuilder.UseSetting(NodeHostDefaults.HostingStartupExcludeAssembliesKey, Configuration[NodeHostDefaults.HostingStartupExcludeAssembliesKey]);
        }

        /// <summary>
        /// Provides information about the node hosting environment a engine is running.
        /// </summary>
        public IHostEnvironment Environment { get; private set; }

        /// <summary>
        /// A collection of services for the engine to compose. This is useful for adding user provided or framework provided services.
        /// </summary>
        public IServiceCollection Services => _hostApplicationBuilder.Services;

        /// <summary>
        /// A collection of configuration providers for the engine to compose. This is useful for adding new configuration sources and providers.
        /// </summary>
        public ConfigurationManager Configuration => _hostApplicationBuilder.Configuration;

        /// <summary>
        /// A collection of logging providers for the engine to compose. This is useful for adding new logging providers.
        /// </summary>
        public ILoggingBuilder Logging => _hostApplicationBuilder.Logging;

        /// <summary>
        /// Allows enabling metrics and directing their output.
        /// </summary>
        public IMetricsBuilder Metrics => _hostApplicationBuilder.Metrics;

        /// <summary>
        /// An <see cref="INodeHostBuilder"/> for configuring server specific properties, but not building.
        /// To build after configuration, call <see cref="Build"/>.
        /// </summary>
        public ConfigureNodeHostBuilder NodeHost { get; private set; }

        /// <summary>
        /// An <see cref="IHostBuilder"/> for configuring host specific properties, but not building.
        /// To build after configuration, call <see cref="Build"/>.
        /// </summary>
        public ConfigureHostBuilder Host { get; private set; }

        IDictionary<object, object> IHostApplicationBuilder.Properties => ((IHostApplicationBuilder)_hostApplicationBuilder).Properties;

        IConfigurationManager IHostApplicationBuilder.Configuration => Configuration;

        IHostEnvironment IHostApplicationBuilder.Environment => Environment;

        /// <summary>
        /// Builds the <see cref="NodeEngine"/>.
        /// </summary>
        /// <returns>A configured <see cref="NodeEngine"/>.</returns>
        public NodeEngine Build()
        {
            // ConfigureContainer callbacks run after ConfigureServices callbacks including the one that adds GenericNodeHostService by default.
            // One nice side effect is this gives a way to configure an IHostedService that starts after the server and stops beforehand.
            _hostApplicationBuilder.Services.Add(_genericNodeHostServiceDescriptor);
            Host.ApplyServiceProviderFactory(_hostApplicationBuilder);
            _builtEngine = new NodeEngine(_hostApplicationBuilder.Build());
            return _builtEngine;
        }

        private void ConfigureEngine(NodeHostBuilderContext context, IEngineBuilder engine) =>
            ConfigureEngine(context, engine, allowDeveloperExceptionPage: true);

        private void ConfigureEngine(NodeHostBuilderContext context, IEngineBuilder engine, bool allowDeveloperExceptionPage)
        {
            Debug.Assert(_builtEngine is not null);

            //// UseRouting called before NodeEngine such as in a StartupFilter
            //// lets remove the property and reset it at the end so we don't mess with the routes in the filter
            //if (engine.Properties.TryGetValue(EndpointRouteBuilderKey, out var priorRouteBuilder))
            //{
            //    engine.Properties.Remove(EndpointRouteBuilderKey);
            //}

            //if (allowDeveloperExceptionPage && context.HostingEnvironment.IsDevelopment())
            //{
            //    //engine.UseDeveloperExceptionPage();
            //}

            //// Wrap the entire destination pipeline in UseRouting() and UseEndpoints(), essentially:
            //// destination.UseRouting()
            //// destination.Run(source)
            //// destination.UseEndpoints()

            //// Set the route builder so that UseRouting will use the NodeEngine as the IClusterNodeBuilder for route matching
            //engine.Properties.Add(NodeEngineHost.GlobalClusterNodeBuilderKey, _builtEngine);

            //// Only call UseRouting() if there are endpoints configured and UseRouting() wasn't called on the global route builder already
            //if (_builtEngine.DataSources.Count > 0)
            //{
            //    // If this is set, someone called UseRouting() when a global route builder was already set
            //    if (!_builtEngine.Properties.TryGetValue(EndpointRouteBuilderKey, out var localRouteBuilder))
            //    {
            //        //engine.UseRouting();
            //        // FrameProcessor the needs to re-route will use this property to call UseRouting()
            //        _builtEngine.Properties[UseRoutingKey] = engine.Properties[UseRoutingKey];
            //    }
            //    else
            //    {
            //        // UseEndpoints will be looking for the RouteBuilder so make sure it's set
            //        engine.Properties[EndpointRouteBuilderKey] = localRouteBuilder;
            //    }
            //}

            //// Process authorization and authentication frame processors independently to avoid
            //// registering frame processors for services that do not exist
            //var serviceProviderIsService = _builtEngine.Services.GetService<IServiceProviderIsService>();
            ////if (serviceProviderIsService?.IsService(typeof(IAuthenticationSchemeProvider)) is true)
            ////{
            ////    // Don't add more than one instance of the frame processor
            ////    if (!_builtEngine.Properties.ContainsKey(AuthenticationFrameProcessorSetKey))
            ////    {
            ////        // The Use invocations will set the property on the outer pipeline,
            ////        // but we want to set it on the inner pipeline as well.
            ////        _builtEngine.Properties[AuthenticationFrameProcessorSetKey] = true;
            ////        engine.UseAuthentication();
            ////    }
            ////}

            ////if (serviceProviderIsService?.IsService(typeof(IAuthorizationHandlerProvider)) is true)
            ////{
            ////    if (!_builtEngine.Properties.ContainsKey(AuthorizationFrameProcessorSetKey))
            ////    {
            ////        _builtEngine.Properties[AuthorizationFrameProcessorSetKey] = true;
            ////        engine.UseAuthorization();
            ////    }
            ////}

            //// Wire the source pipeline to run in the destination pipeline
            //var wireSourcePipeline = new WireSourcePipeline(_builtEngine);
            //engine.Use(wireSourcePipeline.CreateFrameProcessor);

            //if (_builtEngine.DataSources.Count > 0)
            //{
            //    // We don't know if user code called UseEndpoints(), so we will call it just in case, UseEndpoints() will ignore duplicate DataSources
            //    //engine.UseEndpoints(_ => { });
            //}

            //MergeFrameProcessorDescriptions(engine);

            //// Copy the properties to the destination engine builder
            //foreach (var item in _builtEngine.Properties)
            //{
            //    engine.Properties[item.Key] = item.Value;
            //}

            //// Remove the route builder to clean up the properties, we're done adding routes to the pipeline
            //engine.Properties.Remove(NodeEngineHost.GlobalClusterNodeBuilderKey);

            //// reset route builder if it existed, this is needed for StartupFilters
            //if (priorRouteBuilder is not null)
            //{
            //    engine.Properties[EndpointRouteBuilderKey] = priorRouteBuilder;
            //}
        }

        void IHostApplicationBuilder.ConfigureContainer<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory, Action<TContainerBuilder> configure) =>
            _hostApplicationBuilder.ConfigureContainer(factory, configure);

        //private void MergeFrameProcessorDescriptions(IEngineBuilder engine)
        //{
        //    // A user's engine builds up a list of frame processor. Then when the NodeEngine is started, frame processor is automatically added
        //    // if it is required. For example, the engine has mapped endpoints but hasn't configured UseRouting/UseEndpoints.
        //    //
        //    // This method updates the frame processor descriptions to include automatically added frame processor.
        //    // The engine's frame processor list is inserted into the new pipeline to create the best representation possible of the frame processor pipeline.
        //    //
        //    // If the debugger isn't attached then there won't be frame processor description collections in the properties and this does nothing.

        //    Debug.Assert(_builtEngine is not null);

        //    const string FrameProcessorDescriptionsKey = "__FrameProcessorDescriptions";
        //    if (_builtEngine.Properties.TryGetValue(FrameProcessorDescriptionsKey, out var sourceValue) &&
        //        engine.Properties.TryGetValue(FrameProcessorDescriptionsKey, out var destinationValue) &&
        //        sourceValue is List<string> sourceDescriptions &&
        //        destinationValue is List<string> destinationDescriptions)
        //    {
        //        var wireUpIndex = destinationDescriptions.IndexOf(typeof(WireSourcePipeline).FullName!);
        //        if (wireUpIndex != -1)
        //        {
        //            destinationDescriptions.RemoveAt(wireUpIndex);
        //            destinationDescriptions.InsertRange(wireUpIndex, sourceDescriptions);

        //            _builtEngine.Properties[FrameProcessorDescriptionsKey] = destinationDescriptions;
        //        }
        //    }
        //}

        //// This type exists so the place where the source pipeline is wired into the destination pipeline can be identified.
        //private sealed class WireSourcePipeline(IEngineBuilder builtEngine)
        //{
        //    private readonly IEngineBuilder _builtEngine = builtEngine;

        //    public UpdateDelegate CreateFrameProcessor(UpdateDelegate next)
        //    {
        //        _builtEngine.Run(next);
        //        return _builtEngine.Build();
        //    }
        //}
    }
}
