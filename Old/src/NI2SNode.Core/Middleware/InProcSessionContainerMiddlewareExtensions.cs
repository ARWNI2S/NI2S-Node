using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Hosting;
using NI2S.Node.Protocol.Session;

namespace NI2S.Node
{
    public static class InProcSessionContainerMiddlewareExtensions
    {
        public static INodeHostBuilderOld UseInProcSessionContainer(this INodeHostBuilderOld builder)
        {
            return (INodeHostBuilderOld)builder
                .UseMiddleware(s => s.GetRequiredService<InProcSessionContainerMiddleware>())!
                .ConfigureServices((ctx, services) =>
                {
                    services.AddSingleton<InProcSessionContainerMiddleware>();
                    services.AddSingleton<ISessionContainer>((s) => s.GetRequiredService<InProcSessionContainerMiddleware>());
                    services.AddSingleton((s) => s.GetRequiredService<ISessionContainer>().ToAsyncSessionContainer());
                });
        }
    }
}
