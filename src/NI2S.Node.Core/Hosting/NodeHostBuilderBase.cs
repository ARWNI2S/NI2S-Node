using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NI2S.Node.Configuration;
using System;

namespace NI2S.Node.Hosting
{
    internal abstract class NodeHostBuilderBase : INodeHostBuilder, ISupportsUseDefaultServiceProvider
    {
        private protected readonly IHostBuilder _builder;
        private protected readonly IConfiguration _config;

        public NodeHostBuilderBase(IHostBuilder builder, NodeHostBuilderOptions options)
        {
            _builder = builder;
            var configBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection();

            if (!options.SuppressEnvironmentConfiguration)
            {
                configBuilder.AddEnvironmentVariables(prefix: "NETCORE_");
            }

            _config = configBuilder.Build();
        }

        public INodeHost Build()
        {
            throw new NotSupportedException($"Building this implementation of {nameof(INodeHostBuilder)} is not supported.");
        }

        public INodeHostBuilder ConfigureAppConfiguration(Action<NodeHostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            _builder.ConfigureAppConfiguration((context, builder) =>
            {
                var webhostBuilderContext = GetNodeHostBuilderContext(context);
                configureDelegate(webhostBuilderContext, builder);
            });

            return this;
        }

        public INodeHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            return ConfigureServices((context, services) => configureServices(services));
        }

        public INodeHostBuilder ConfigureServices(Action<NodeHostBuilderContext, IServiceCollection> configureServices)
        {
            _builder.ConfigureServices((context, builder) =>
            {
                var webhostBuilderContext = GetNodeHostBuilderContext(context);
                configureServices(webhostBuilderContext, builder);
            });

            return this;
        }

        public INodeHostBuilder UseDefaultServiceProvider(Action<NodeHostBuilderContext, ServiceProviderOptions> configure)
        {
            _builder.UseServiceProviderFactory(context =>
            {
                var webHostBuilderContext = GetNodeHostBuilderContext(context);
                var options = new ServiceProviderOptions();
                configure(webHostBuilderContext, options);
                return new DefaultServiceProviderFactory(options);
            });

            return this;
        }

        protected NodeHostBuilderContext GetNodeHostBuilderContext(HostBuilderContext context)
        {
            if (!context.Properties.TryGetValue(typeof(NodeHostBuilderContext), out var contextVal))
            {
                // Use _config as a fallback for NodeHostOptions in case the chained source was removed from the hosting IConfigurationBuilder.
                var options = new NodeHostOptions(context.Configuration, fallbackConfiguration: _config, environment: context.HostingEnvironment);
                var webHostBuilderContext = new NodeHostBuilderContext
                {
                    Configuration = context.Configuration,
                    HostingEnvironment = new HostingEnvironment(),
                };
                webHostBuilderContext.HostingEnvironment.Initialize(context.HostingEnvironment.ContentRootPath, options, baseEnvironment: context.HostingEnvironment);
                context.Properties[typeof(NodeHostBuilderContext)] = webHostBuilderContext;
                context.Properties[typeof(NodeHostOptions)] = options;
                return webHostBuilderContext;
            }

            // Refresh config, it's periodically updated/replaced
            var webHostContext = (NodeHostBuilderContext)contextVal;
            webHostContext.Configuration = context.Configuration;
            return webHostContext;
        }

        public string GetSetting(string key)
        {
            return _config[key];
        }

        public INodeHostBuilder UseSetting(string key, string value)
        {
            _config[key] = value;
            return this;
        }
    }
}
