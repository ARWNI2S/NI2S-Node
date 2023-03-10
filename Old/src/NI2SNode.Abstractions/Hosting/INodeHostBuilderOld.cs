using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NI2S.Node.Middleware;
using NI2S.Node.Protocol;
using NI2S.Node.Protocol.Session;

namespace NI2S.Node.Hosting
{
    public interface INodeHostBuilderOld : IHostBuilder, IMinimalApiHostBuilderOld
    {
        INodeHostBuilderOld ConfigureSupplementServices(Action<HostBuilderContext, IServiceCollection> configureDelegate);
    }

    public interface INodeHostBuilderOld<TReceivePackage> : INodeHostBuilderOld
    {
        INodeHostBuilderOld<TReceivePackage> ConfigureServerOptions(Func<HostBuilderContext, IConfiguration, IConfiguration> serverOptionsReader);

        new INodeHostBuilderOld<TReceivePackage> ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate);

        new INodeHostBuilderOld<TReceivePackage> ConfigureSupplementServices(Action<HostBuilderContext, IServiceCollection> configureDelegate);

        INodeHostBuilderOld<TReceivePackage> UseMiddleware<TMiddleware>()
            where TMiddleware : class, IMiddleware;

        INodeHostBuilderOld<TReceivePackage> UsePipelineFilter<TPipelineFilter>()
            where TPipelineFilter : IPipelineFilter<TReceivePackage>, new();

        INodeHostBuilderOld<TReceivePackage> UsePipelineFilterFactory<TPipelineFilterFactory>()
            where TPipelineFilterFactory : class, IPipelineFilterFactory<TReceivePackage>;

        INodeHostBuilderOld<TReceivePackage> UseHostedService<THostedService>()
            where THostedService : class, IHostedService;

        INodeHostBuilderOld<TReceivePackage> UsePackageDecoder<TPackageDecoder>()
            where TPackageDecoder : class, IPackageDecoder<TReceivePackage>;

        INodeHostBuilderOld<TReceivePackage> UsePackageHandlingScheduler<TPackageHandlingScheduler>()
            where TPackageHandlingScheduler : class, IPackageHandlingScheduler<TReceivePackage>;

        INodeHostBuilderOld<TReceivePackage> UseSessionFactory<TSessionFactory>()
            where TSessionFactory : class, ISessionFactory;

        INodeHostBuilderOld<TReceivePackage> UseSession<TSession>()
            where TSession : ISession;

        INodeHostBuilderOld<TReceivePackage> UsePackageHandlingContextAccessor();
    }
}