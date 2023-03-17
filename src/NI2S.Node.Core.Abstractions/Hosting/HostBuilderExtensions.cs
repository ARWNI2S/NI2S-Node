using Microsoft.Extensions.Hosting;

namespace NI2S.Node.Hosting
{
    public static class HostBuilderExtensions
    {
        //public static INodeHostBuilder? AsNodeHostBuilder(this IHostBuilder hostBuilder)
        //{
        //    return hostBuilder as INodeHostBuilder;
        //}

        //public static INodeHostBuilder UseMiddleware<TMiddleware>(this INodeHostBuilder builder)
        //    where TMiddleware : class, IMiddleware
        //{
        //    return builder.ConfigureServices((ctx, services) =>
        //    {
        //        services.TryAddEnumerable(ServiceDescriptor.Singleton<IMiddleware, TMiddleware>());
        //    }).AsNodeHostBuilder();
        //}

        //public static INodeHostBuilder UseMiddleware<TMiddleware>(this INodeHostBuilder builder, Func<IServiceProvider, TMiddleware> implementationFactory)
        //    where TMiddleware : class, IMiddleware
        //{
        //    return builder.ConfigureServices((ctx, services) =>
        //    {
        //        services.TryAddEnumerable(ServiceDescriptor.Singleton<IMiddleware, TMiddleware>(implementationFactory));
        //    }).AsNodeHostBuilder();
        //}

        //public static INodeHostBuilder UseChannelCreatorFactory<TChannelCreatorFactory>(this INodeHostBuilder builder)
        //    where TChannelCreatorFactory : class, IChannelCreatorFactory
        //{
        //    return builder.ConfigureServices((ctx, services) =>
        //    {
        //        services.AddSingleton<IChannelCreatorFactory, TChannelCreatorFactory>();
        //    }).AsNodeHostBuilder();
        //}
    }
}
