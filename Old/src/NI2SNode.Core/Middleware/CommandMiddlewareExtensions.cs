using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Command;
using NI2S.Node.Configuration.Options;
using NI2S.Node.Hosting;
using NI2S.Node.Protocol;

namespace NI2S.Node
{
    public static class CommandMiddlewareExtensions
    {
        public static Type GetKeyType<TPackageInfo>()
        {
            var interfaces = typeof(TPackageInfo).GetInterfaces();
            var keyInterface = interfaces.FirstOrDefault(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IKeyedPackageInfo<>));

            return keyInterface == null
                ? throw new Exception($"The package type {nameof(TPackageInfo)} should implement the interface {typeof(IKeyedPackageInfo<>).Name}.")
                : keyInterface.GetGenericArguments().FirstOrDefault()!;
        }

        private static INodeHostBuilderOld ConfigureCommand(this INodeHostBuilderOld builder)
        {
            return (INodeHostBuilderOld)builder.ConfigureServices((hostCxt, services) =>
                {
                    services.Configure<CommandOptions>(hostCxt.Configuration?.GetSection("serverOptions")?.GetSection("commands")!);
                });
        }

        public static INodeHostBuilderOld<TPackageInfo> UseCommand<TPackageInfo>(this INodeHostBuilderOld<TPackageInfo> builder)
            where TPackageInfo : class
        {
            var keyType = GetKeyType<TPackageInfo>();

            var useCommandMethod = typeof(CommandMiddlewareExtensions).GetMethod("UseCommand", new Type[] { typeof(INodeHostBuilderOld) });
            useCommandMethod = useCommandMethod?.MakeGenericMethod(keyType, typeof(TPackageInfo));

            var hostBuilder = useCommandMethod?.Invoke(null, new object[] { builder }) as INodeHostBuilderOld;
            return (INodeHostBuilderOld<TPackageInfo>)hostBuilder!.ConfigureCommand();
        }

        public static INodeHostBuilderOld<TPackageInfo> UseCommand<TPackageInfo>(this INodeHostBuilderOld<TPackageInfo> builder, Action<CommandOptions> configurator)
            where TPackageInfo : class
        {
            return builder.UseCommand()
               .ConfigureServices((hostCtx, services) =>
               {
                   services.Configure(configurator);
               });
        }

        public static INodeHostBuilderOld<TPackageInfo> UseCommand<TKey, TPackageInfo>(this INodeHostBuilderOld<TPackageInfo> builder, Action<CommandOptions> configurator, IEqualityComparer<TKey> comparer)
            where TPackageInfo : class, IKeyedPackageInfo<TKey>
        {
            return builder.UseCommand(configurator)
                .ConfigureServices((hostCtx, services) =>
                {
                    services.AddSingleton(comparer);
                });
        }

        public static INodeHostBuilderOld<TPackageInfo> UseCommand<TKey, TPackageInfo>(this INodeHostBuilderOld builder)
            where TPackageInfo : class, IKeyedPackageInfo<TKey>
            where TKey : notnull
        {
            return (INodeHostBuilderOld<TPackageInfo>)builder.UseMiddleware<CommandMiddleware<TKey, TPackageInfo>>()!
                .ConfigureCommand();
        }

        public static INodeHostBuilderOld<TPackageInfo> UseCommand<TKey, TPackageInfo>(this INodeHostBuilderOld builder, Action<CommandOptions> configurator)
            where TPackageInfo : class, IKeyedPackageInfo<TKey>
            where TKey : notnull
        {
            return builder.UseCommand<TKey, TPackageInfo>()
               .ConfigureServices((hostCtx, services) =>
               {
                   services.Configure(configurator);
               });
        }

        public static INodeHostBuilderOld<TPackageInfo> UseCommand<TKey, TPackageInfo>(this INodeHostBuilderOld builder, Action<CommandOptions> configurator, IEqualityComparer<TKey> comparer)
            where TPackageInfo : class, IKeyedPackageInfo<TKey>
            where TKey : notnull
        {
            return builder.UseCommand<TKey, TPackageInfo>(configurator)
                .ConfigureServices((hostCtx, services) =>
                {
                    services.AddSingleton(comparer);
                });
        }
    }
}
