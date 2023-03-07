using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NI2S.Node.Middleware;
using NI2S.Node.Protocol.Channel;

namespace NI2S.Node.Hosting
{
    public static class HostBuilderExtensions
    {
        public static INodeHostBuilder? AsSuperSocketBuilder(this IHostBuilder hostBuilder)
        {
            return hostBuilder as INodeHostBuilder;
        }

        public static INodeHostBuilder? UseMiddleware<TMiddleware>(this INodeHostBuilder builder)
            where TMiddleware : class, IMiddleware
        {
            return builder.ConfigureServices((ctx, services) =>
            {
                services.TryAddEnumerable(ServiceDescriptor.Singleton<IMiddleware, TMiddleware>());
            }).AsSuperSocketBuilder();
        }

        public static INodeHostBuilder? UseMiddleware<TMiddleware>(this INodeHostBuilder builder, Func<IServiceProvider, TMiddleware> implementationFactory)
            where TMiddleware : class, IMiddleware
        {
            return builder.ConfigureServices((ctx, services) =>
            {
                services.TryAddEnumerable(ServiceDescriptor.Singleton<IMiddleware, TMiddleware>(implementationFactory));
            }).AsSuperSocketBuilder();
        }
        public static INodeHostBuilder? UseChannelCreatorFactory<TChannelCreatorFactory>(this INodeHostBuilder builder)
            where TChannelCreatorFactory : class, IChannelCreatorFactory
        {
            return builder.ConfigureServices((ctx, services) =>
            {
                services.AddSingleton<IChannelCreatorFactory, TChannelCreatorFactory>();
            }).AsSuperSocketBuilder();
        }
    }
}
