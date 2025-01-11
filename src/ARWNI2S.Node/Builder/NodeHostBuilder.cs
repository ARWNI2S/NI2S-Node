// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Configuration;
using ARWNI2S.Engine.Builder;
using ARWNI2S.Engine.Hosting;
using ARWNI2S.Hosting;
using ARWNI2S.Hosting.Builder;
using ARWNI2S.Node.Extensibility;
using ARWNI2S.Node.Hosting;
using ARWNI2S.Node.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace ARWNI2S.Node.Builder
{
    public sealed class NodeHostBuilder : IHostApplicationBuilder
    {
        private readonly HostApplicationBuilder _hostApplicationBuilder;
        private readonly ServiceDescriptor _genericNI2SNodeServiceDescriptor;
        private NI2SNode _builtNode;

        /// <summary>
        /// A collection of configuration providers for the application to compose. This is useful for adding new configuration sources and providers.
        /// </summary>
        public ConfigurationManager Configuration => _hostApplicationBuilder.Configuration;

        /// <summary>
        /// Provides information about the NI2S hosting environment a node engine is running.
        /// </summary>
        public INiisHostEnvironment Environment { get; private set; }

        /// <summary>
        /// Provides information about the NI2S hosting environment a node engine is running.
        /// </summary>
        public NI2SSettings Settings { get; private set; }

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
        /// An <see cref="IHostBuilder"/> for configuring host specific properties, but not building.
        /// To build after configuration, call <see cref="Build"/>.
        /// </summary>
        public ConfigureHostBuilder Host { get; private set; }
        /// <summary>
        /// An <see cref="INiisHostBuilder"/> for configuring server specific properties, but not building.
        /// To build after configuration, call <see cref="Build"/>.
        /// </summary>
        public ConfigureNI2SHostBuilder NI2SHost { get; private set; }

        internal NodeHostBuilder(string[] args = null)
        {
            var configuration = new ConfigurationManager();

            _hostApplicationBuilder = new HostApplicationBuilder(new HostApplicationBuilderSettings
            {
                Args = args,
                Configuration = configuration,
            });

            // Run methods to configure web host defaults early to populate services
            var bootstrapHostBuilder = new BootstrapHostBuilder(_hostApplicationBuilder);

            bootstrapHostBuilder.ConfigureNI2SHostingDefaults(niisHostBuilder =>
            {
                // Runs inline.
                niisHostBuilder.Configure(ConfigureEngine);

                InitializeHostSettings(niisHostBuilder);
            });

            _genericNI2SNodeServiceDescriptor = InitializeHosting(bootstrapHostBuilder);
        }

        private void InitializeHostSettings(INiisHostBuilder niisHostBuilder)
        {
            niisHostBuilder.UseSetting(NI2SHostingDefaults.ApplicationKey, _hostApplicationBuilder.Environment.ApplicationName ?? "");
        }

        [MemberNotNull(nameof(Environment), nameof(Settings), nameof(Host), nameof(NI2SHost))]
        private ServiceDescriptor InitializeHosting(BootstrapHostBuilder bootstrapHostBuilder)
        {
            // This applies the config from ConfigureNI2SHostingDefaults
            // Grab the GenericNI2SHostService ServiceDescriptor so we can append it after any user-added IHostedServices during Build();
            var genericNI2SHostServiceDescriptor = bootstrapHostBuilder.RunDefaultCallbacks();

            // Grab the NI2SHostBuilderContext from the property bag to use in the ConfigureNodeHostBuilder. Then
            // grab the INiisHostEnvironment from the niisHostContext. This also matches the instance in the IServiceCollection.
            var nodeHostContext = (NI2SHostBuilderContext)bootstrapHostBuilder.Properties[typeof(NI2SHostBuilderContext)];
            Environment = nodeHostContext.HostingEnvironment;
            Settings = (NI2SSettings)bootstrapHostBuilder.Properties[typeof(NI2SSettings)];

            Host = new ConfigureHostBuilder(bootstrapHostBuilder.Context, Configuration, Services);
            NI2SHost = new ConfigureNI2SHostBuilder(nodeHostContext, Configuration, Services);

            return genericNI2SHostServiceDescriptor;
        }

        IDictionary<object, object> IHostApplicationBuilder.Properties => ((IHostApplicationBuilder)_hostApplicationBuilder).Properties;

        IConfigurationManager IHostApplicationBuilder.Configuration => Configuration;

        IHostEnvironment IHostApplicationBuilder.Environment => Environment;

        public NI2SNode Build()
        {
            // One nice side effect is this gives a way to configure an IHostedService that starts after the server and stops beforehand.
            _hostApplicationBuilder.Services.Add(_genericNI2SNodeServiceDescriptor);
            Host.ApplyServiceProviderFactory(_hostApplicationBuilder);
            _builtNode = new NI2SNode(_hostApplicationBuilder.Build());
            return _builtNode;
        }

        private void ConfigureEngine(NI2SHostBuilderContext context, IEngineBuilder engine) =>
            ConfigureEngine(context, engine, enableExceptionReport: true);

        private void ConfigureEngine(NI2SHostBuilderContext context, IEngineBuilder engine, bool enableExceptionReport)
        {
            Debug.Assert(_builtNode is not null);

            // UseRouting called before WebApplication such as in a StartupFilter
            // lets remove the property and reset it at the end so we don't mess with the routes in the filter
            if (engine.Properties.TryGetValue(NI2SHostingDefaults.NodeBuilderKey, out var priorEngineBuilder))
            {
                engine.Properties.Remove(NI2SHostingDefaults.NodeBuilderKey);
            }

            if (enableExceptionReport && context.HostingEnvironment.IsDevelopment())
            {
                //TODO URGENT
                //engine.UseDeveloperExceptionReport();
            }

            engine.Properties.Add(NI2SHostingDefaults.GlobalNodeBuilderKey, _builtNode);

            context.EngineContext.InitializeContext(_builtNode);
            engine.ConfigureEngine();

            // Only call UseModules() if there are modules configured and UseModules() wasn't called on the global node builder already
            if (_builtNode.DataSources.Count > 0)
            {
                // If this is set, someone called UseRouting() when a global node builder was already set
                if (!_builtNode.Properties.TryGetValue(NI2SHostingDefaults.NodeBuilderKey, out var localNodeBuilder))
                {
                    engine.UseModules();
                    //// Middleware the needs to re-route will use this property to call UseRouting()
                    _builtNode.Properties[NI2SHostingDefaults.UseModulesKey] = engine.Properties[NI2SHostingDefaults.UseModulesKey];
                }
                else
                {
                    // UseModules will be looking for the NodeBuilder so make sure it's set
                    engine.Properties[NI2SHostingDefaults.NodeBuilderKey] = localNodeBuilder;
                }
            }

            // Copy the properties to the destination engine builder
            foreach (var item in _builtNode.Properties)
            {
                engine.Properties[item.Key] = item.Value;
            }

            // Remove the route builder to clean up the properties, we're done adding routes to the pipeline
            engine.Properties.Remove(NI2SHostingDefaults.GlobalNodeBuilderKey);

            // Reset route builder if it existed, this is needed for StartupFilters
            if (priorEngineBuilder is not null)
            {
                engine.Properties[NI2SHostingDefaults.NodeBuilderKey] = priorEngineBuilder;
            }
        }

        void IHostApplicationBuilder.ConfigureContainer<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory, Action<TContainerBuilder> configure) =>
            _hostApplicationBuilder.ConfigureContainer(factory, configure);
    }
}