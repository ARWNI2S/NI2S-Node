// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.Hosting;
using System;

namespace NI2S.Node.Hosting.Builder
{
    /// <summary>
    /// A builder for NI2S node engine hosting and services.
    /// </summary>
    public sealed class NodeEngineHostBuilder
    {
        NodeEngineHost _builtNodeEngineHost;

        internal NodeEngineHostBuilder(NodeEngineHostOptions options, Action<IHostBuilder> configureDefaults = null)
        {
            //var configuration = new ConfigurationManager();

            //configuration.AddEnvironmentVariables(prefix: "DOTNET_");

            //_hostEngineBuilder = new HostApplicationBuilder(new HostApplicationBuilderSettings
            //{
            //    Args = options.Args,
            //    ApplicationName = options.ApplicationName,
            //    EnvironmentName = options.EnvironmentName,
            //    ContentRootPath = options.ContentRootPath,
            //    Configuration = configuration,
            //});

            //// Set NodeRootPath if necessary
            //if (options.NodeRootPath is not null)
            //{
            //    Configuration.AddInMemoryCollection(new[]
            //    {
            //        new KeyValuePair<string, string>(NodeHostDefaults.NodeRootKey, options.NodeRootPath),
            //    });
            //}

            //// Run methods to configure web host defaults early to populate services
            //var bootstrapHostBuilder = new BootstrapHostBuilder(_hostEngineBuilder);

            //// This is for testing purposes
            //configureDefaults?.Invoke(bootstrapHostBuilder);

            //bootstrapHostBuilder.ConfigureNodeHostDefaults(nodeHostBuilder =>
            //{
            //    // Runs inline.
            //    nodeHostBuilder.Configure(ConfigureEngine);

            //    nodeHostBuilder.UseSetting(NodeHostDefaults.ApplicationKey, _hostEngineBuilder.Environment.ApplicationName ?? "");
            //    nodeHostBuilder.UseSetting(NodeHostDefaults.PreventHostingStartupKey, Configuration[NodeHostDefaults.PreventHostingStartupKey]);
            //    nodeHostBuilder.UseSetting(NodeHostDefaults.HostingStartupAssembliesKey, Configuration[NodeHostDefaults.HostingStartupAssembliesKey]);
            //    nodeHostBuilder.UseSetting(NodeHostDefaults.HostingStartupExcludeAssembliesKey, Configuration[NodeHostDefaults.HostingStartupExcludeAssembliesKey]);
            //},
            //options =>
            //{
            //    // We've already applied "DOTNET_" environment variables to hosting config
            //    options.SuppressEnvironmentConfiguration = true;
            //});

            //// This applies the config from ConfigureNodeHostDefaults
            //// Grab the GenericNodeHostService ServiceDescriptor so we can append it after any user-added IHostedServices during Build();
            //_genericNodeHostServiceDescriptor = bootstrapHostBuilder.RunDefaultCallbacks();

            //// Grab the NodeHostBuilderContext from the property bag to use in the ConfigureNodeHostBuilder. Then
            //// grab the INodeHostEnvironment from the nodeHostContext. This also matches the instance in the IServiceCollection.
            //var nodeHostContext = (NodeHostBuilderContext)bootstrapHostBuilder.Properties[typeof(NodeHostBuilderContext)];
            //Environment = nodeHostContext.HostingEnvironment;

            //Host = new ConfigureHostBuilder(bootstrapHostBuilder.Context, Configuration, Services);
            //NodeHost = new ConfigureNodeHostBuilder(nodeHostContext, Configuration, Services);
        }




        /// <summary>
        /// Builds the <see cref="NodeEngineHost"/>.
        /// </summary>
        /// <returns>A configured <see cref="NodeEngineHost"/>.</returns>
        public NodeEngineHost Build()
        {
            // ConfigureContainer callbacks run after ConfigureServices callbacks including the one that adds GenericNodeHostService by default.
            // One nice side effect is this gives a way to configure an IHostedService that starts after the server and stops beforehand.
            //_hostEngineBuilder.Services.Add(_genericNodeHostServiceDescriptor);
            //Host.ApplyServiceProviderFactory(_hostEngineBuilder);
            //_builtEngine = new NodeEngine(_hostEngineBuilder.Build());
            return _builtNodeEngineHost;
        }

    }
}
