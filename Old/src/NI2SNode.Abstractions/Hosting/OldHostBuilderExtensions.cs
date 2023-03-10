using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NI2S.Node.Middleware;
using NI2S.Node.Protocol.Channel;

namespace NI2S.Node.Hosting
{
    public static class OldHostBuilderExtensions
    {
        public static INodeHostBuilderOld? AsNodeHostBuilder(this IHostBuilder hostBuilder)
        {
            return hostBuilder as INodeHostBuilderOld;
        }

        public static INodeHostBuilderOld? UseMiddleware<TMiddleware>(this INodeHostBuilderOld builder)
            where TMiddleware : class, IMiddleware
        {
            return builder.ConfigureServices((ctx, services) =>
            {
                services.TryAddEnumerable(ServiceDescriptor.Singleton<IMiddleware, TMiddleware>());
            }).AsNodeHostBuilder();
        }

        public static INodeHostBuilderOld? UseMiddleware<TMiddleware>(this INodeHostBuilderOld builder, Func<IServiceProvider, TMiddleware> implementationFactory)
            where TMiddleware : class, IMiddleware
        {
            return builder.ConfigureServices((ctx, services) =>
            {
                services.TryAddEnumerable(ServiceDescriptor.Singleton<IMiddleware, TMiddleware>(implementationFactory));
            }).AsNodeHostBuilder();
        }
        public static INodeHostBuilderOld? UseChannelCreatorFactory<TChannelCreatorFactory>(this INodeHostBuilderOld builder)
            where TChannelCreatorFactory : class, IChannelCreatorFactory
        {
            return builder.ConfigureServices((ctx, services) =>
            {
                services.AddSingleton<IChannelCreatorFactory, TChannelCreatorFactory>();
            }).AsNodeHostBuilder();
        }
    }
}
