using ARWNI2S.Node.Builder;
using ARWNI2S.Node.Hosting.Configuration;
using ARWNI2S.Node.Hosting.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;

namespace ARWNI2S.Node.Hosting.Internal
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
                configBuilder.AddEnvironmentVariables(prefix: "ARWNI2S_");
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
                var nodehostBuilderContext = GetNodeHostBuilderContext(context);
                configureDelegate(nodehostBuilderContext, builder);
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
                var nodehostBuilderContext = GetNodeHostBuilderContext(context);
                configureServices(nodehostBuilderContext, builder);
            });

            return this;
        }

        public INodeHostBuilder UseDefaultServiceProvider(Action<NodeHostBuilderContext, ServiceProviderOptions> configure)
        {
            _builder.UseServiceProviderFactory(context =>
            {
                var nodeNodeHostBuilderContext = GetNodeHostBuilderContext(context);
                var options = new ServiceProviderOptions();
                configure(nodeNodeHostBuilderContext, options);
                return new DefaultServiceProviderFactory(options);
            });

            return this;
        }

        protected NodeHostBuilderContext GetNodeHostBuilderContext(NodeHostBuilderContext context)
        {
            if (!context.Properties.TryGetValue(typeof(NodeHostBuilderContext), out var contextVal))
            {
                // Use _config as a fallback for NodeHostOptions in case the chained source was removed from the hosting IConfigurationBuilder.
                var options = new NodeHostOptions(context.Configuration, fallbackConfiguration: _config, environment: context.HostingEnvironment);
                var nodeNodeHostBuilderContext = new NodeHostBuilderContext
                {
                    Configuration = context.Configuration,
                    HostingEnvironment = new HostingEnvironment(),
                };
                nodeNodeHostBuilderContext.HostingEnvironment.Initialize(context.HostingEnvironment.ContentRootPath, options, baseEnvironment: context.HostingEnvironment);
                context.Properties[typeof(NodeHostBuilderContext)] = nodeNodeHostBuilderContext;
                context.Properties[typeof(NodeHostOptions)] = options;
                return nodeNodeHostBuilderContext;
            }

            // Refresh config, it's periodically updated/replaced
            var nodeHostContext = (NodeHostBuilderContext)contextVal;
            nodeHostContext.Configuration = context.Configuration;
            return nodeHostContext;
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