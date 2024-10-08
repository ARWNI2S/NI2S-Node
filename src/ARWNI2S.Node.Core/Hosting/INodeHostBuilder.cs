﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Node.Hosting
{
    public interface INodeHostBuilder : IHostBuilder, IMinimalApiHostBuilder
    {
        INodeHostBuilder ConfigureSupplementServices(Action<HostBuilderContext, IServiceCollection> configureDelegate);

        INodeHostBuilder ConfigureNodeOptions(Func<HostBuilderContext, IConfiguration, IConfiguration> serverOptionsReader);

        new INodeHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate);

        //new INodeHostBuilder ConfigureSupplementServices(Action<HostBuilderContext, IServiceCollection> configureDelegate);

        //INodeHostBuilder UseMiddleware<TMiddleware>()
        //    where TMiddleware : class, IMiddleware;

        //INodeHostBuilder UsePipelineFilter<TPipelineFilter>()
        //    where TPipelineFilter : IPipelineFilter, new();

        //INodeHostBuilder UsePipelineFilterFactory<TPipelineFilterFactory>()
        //    where TPipelineFilterFactory : class, IPipelineFilterFactory;

        INodeHostBuilder UseHostedService<THostedService>()
            where THostedService : class, IHostedService;

        //INodeHostBuilder UsePackageDecoder<TPackageDecoder>()
        //    where TPackageDecoder : class, IPackageDecoder;

        //INodeHostBuilder UsePackageEncoder<TPackageEncoder>()
        //    where TPackageEncoder : class, IPackageEncoder;

        //INodeHostBuilder UsePackageHandlingScheduler<TPackageHandlingScheduler>()
        //    where TPackageHandlingScheduler : class, IPackageHandlingScheduler;

        //INodeHostBuilder UseSessionFactory<TSessionFactory>()
        //    where TSessionFactory : class, ISessionFactory;

        //INodeHostBuilder UseSession<TSession>()
        //    where TSession : IAppSession;

        INodeHostBuilder UsePackageHandlingContextAccessor();
    }
}
