using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Node.Hosting
{
    public class NodeHostBuilder : HostBuilderAdapter<NodeHostBuilder>, INodeHostBuilder, IHostBuilder
    {
        //protected List<Action<HostBuilderContext, IServiceCollection>> ConfigureServicesActions { get; private set; } = [];

        //protected List<Action<HostBuilderContext, IServiceCollection>> ConfigureSupplementServicesActions = [];





        public NodeHostBuilder(IHostBuilder hostBuilder)
            : base(hostBuilder) { }

        public NodeHostBuilder()
            : this(args: null) { }

        public NodeHostBuilder(string[] args)
            : base(args) { }















        public INodeHostBuilder ConfigureSupplementServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            return this;
        }

        public INodeHostBuilder ConfigureNodeOptions(Func<HostBuilderContext, IConfiguration, IConfiguration> nodeOptionsReader)
        {
            return this;
        }

        public INodeHostBuilder UsePackageHandlingContextAccessor()
        {
            return this;
        }

        INodeHostBuilder INodeHostBuilder.ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            return this;
        }

        INodeHostBuilder INodeHostBuilder.UseHostedService<THostedService>()
        {
            return this;
        }







        private void ConfigureHostBuilder()
        {
            HostBuilder.ConfigureServices((ctx, services) =>
            {
                //RegisterBasicServices(ctx, services, services);
            }).ConfigureServices((ctx, services) =>
            {
                //foreach (var action in ConfigureServicesActions)
                //{
                //    action(ctx, services);
                //}

                //foreach (var action in ConfigureSupplementServicesActions)
                //{
                //    action(ctx, services);
                //}
            }).ConfigureServices((ctx, services) =>
            {
                //RegisterDefaultServices(ctx, services, services);
            });
        }

        /// <inheritdoc/>
        void IMinimalApiHostBuilder.ConfigureHostBuilder() => ConfigureHostBuilder();















        public static INodeHostBuilder Create() { return Create(null); }

        public static INodeHostBuilder Create(string[] args) { return new NodeHostBuilder(args); }
    }
}
