using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NI2S.Node.Middleware;
using NI2S.Node.Protocol;
using NI2S.Node.Protocol.Session;

namespace NI2S.Node.Hosting
{
    public interface INodeHostBuilder : IHostBuilder, IMinimalApiHostBuilder
    {
        INodeHostBuilder ConfigureSupplementServices(Action<HostBuilderContext, IServiceCollection> configureDelegate);
    }

    public interface INodeHostBuilder<TReceivePackage> : INodeHostBuilder
    {
        INodeHostBuilder<TReceivePackage> ConfigureServerOptions(Func<HostBuilderContext, IConfiguration, IConfiguration> serverOptionsReader);

        new INodeHostBuilder<TReceivePackage> ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate);

        new INodeHostBuilder<TReceivePackage> ConfigureSupplementServices(Action<HostBuilderContext, IServiceCollection> configureDelegate);

        INodeHostBuilder<TReceivePackage> UseMiddleware<TMiddleware>()
            where TMiddleware : class, IMiddleware;

        INodeHostBuilder<TReceivePackage> UsePipelineFilter<TPipelineFilter>()
            where TPipelineFilter : IPipelineFilter<TReceivePackage>, new();

        INodeHostBuilder<TReceivePackage> UsePipelineFilterFactory<TPipelineFilterFactory>()
            where TPipelineFilterFactory : class, IPipelineFilterFactory<TReceivePackage>;

        INodeHostBuilder<TReceivePackage> UseHostedService<THostedService>()
            where THostedService : class, IHostedService;

        INodeHostBuilder<TReceivePackage> UsePackageDecoder<TPackageDecoder>()
            where TPackageDecoder : class, IPackageDecoder<TReceivePackage>;

        INodeHostBuilder<TReceivePackage> UsePackageHandlingScheduler<TPackageHandlingScheduler>()
            where TPackageHandlingScheduler : class, IPackageHandlingScheduler<TReceivePackage>;

        INodeHostBuilder<TReceivePackage> UseSessionFactory<TSessionFactory>()
            where TSessionFactory : class, ISessionFactory;

        INodeHostBuilder<TReceivePackage> UseSession<TSession>()
            where TSession : ISession;

        INodeHostBuilder<TReceivePackage> UsePackageHandlingContextAccessor();
    }
}