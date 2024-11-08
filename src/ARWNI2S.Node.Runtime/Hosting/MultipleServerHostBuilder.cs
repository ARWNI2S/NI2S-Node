using ARWNI2S.Engine.Network;
using ARWNI2S.Engine.Network.Host;
using ARWNI2S.Infrastructure.Network.Protocol;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Runtime.Hosting
{
    public class MultipleServerHostBuilder : HostBuilderAdapter<MultipleServerHostBuilder>, IMinimalApiHostBuilder
    {
        private List<IServerHostBuilderAdapter> _hostBuilderAdapters = [];

        private MultipleServerHostBuilder()
            : this(args: null)
        {

        }

        private MultipleServerHostBuilder(string[] args)
            : base(args)
        {

        }

        internal MultipleServerHostBuilder(IHostBuilder hostBuilder)
            : base(hostBuilder)
        {

        }

        protected virtual void ConfigureServers(HostBuilderContext context, IServiceCollection hostServices)
        {
            foreach (var adapter in _hostBuilderAdapters)
            {
                adapter.ConfigureServer(context, hostServices);
            }
        }

        public override IHost Build()
        {
            ConfigureServices(ConfigureServers);

            var host = base.Build();
            var services = host.Services;

            AdaptMultipleServerHost(services);

            return host;
        }

        internal void AdaptMultipleServerHost(IServiceProvider services)
        {
            foreach (var adapter in _hostBuilderAdapters)
            {
                adapter.ConfigureServiceProvider(services);
            }
        }

        public static MultipleServerHostBuilder Create()
        {
            return Create(args: null);
        }

        public static MultipleServerHostBuilder Create(string[] args)
        {
            return new MultipleServerHostBuilder(args);
        }

        private ServerHostBuilderAdapter<TReceivePackage> CreateServerHostBuilder<TReceivePackage>(Action<NodeServerHostBuilder<TReceivePackage>> hostBuilderDelegate)
            where TReceivePackage : class
        {
            var hostBuilder = new ServerHostBuilderAdapter<TReceivePackage>(this);
            hostBuilderDelegate(hostBuilder);
            return hostBuilder;
        }

        public MultipleServerHostBuilder AddServer<TReceivePackage>(Action<INodeServerHostBuilder<TReceivePackage>> hostBuilderDelegate)
            where TReceivePackage : class
        {
            var hostBuilder = CreateServerHostBuilder(hostBuilderDelegate);
            _hostBuilderAdapters.Add(hostBuilder);
            return this;
        }

        public MultipleServerHostBuilder AddServer<TReceivePackage, TPipelineFilter>(Action<INodeServerHostBuilder<TReceivePackage>> hostBuilderDelegate)
            where TReceivePackage : class
            where TPipelineFilter : IPipelineFilter<TReceivePackage>, new()
        {
            var hostBuilder = CreateServerHostBuilder(hostBuilderDelegate);
            _hostBuilderAdapters.Add(hostBuilder);
            hostBuilder.UsePipelineFilter<TPipelineFilter>();
            return this;
        }

        public MultipleServerHostBuilder AddServer(IServerHostBuilderAdapter hostBuilderAdapter)
        {
            _hostBuilderAdapters.Add(hostBuilderAdapter);
            return this;
        }

        public MultipleServerHostBuilder AddServer<TNodeServerService, TReceivePackage, TPipelineFilter>(Action<NodeServerHostBuilder<TReceivePackage>> hostBuilderDelegate)
            where TReceivePackage : class
            where TPipelineFilter : IPipelineFilter<TReceivePackage>, new()
            where TNodeServerService : NodeServerService<TReceivePackage>
        {
            var hostBuilder = CreateServerHostBuilder(hostBuilderDelegate);

            _hostBuilderAdapters.Add(hostBuilder);

            hostBuilder
                .UsePipelineFilter<TPipelineFilter>()
                .UseHostedService<TNodeServerService>();
            return this;
        }

        public IMinimalApiHostBuilder AsMinimalApiHostBuilder()
        {
            return this as IMinimalApiHostBuilder;
        }

        void IMinimalApiHostBuilder.ConfigureHostBuilder()
        {
            ConfigureServices(ConfigureServers);
            HostBuilder.ConfigureServices((_, services) => services.AddSingleton(this));
        }
    }
}