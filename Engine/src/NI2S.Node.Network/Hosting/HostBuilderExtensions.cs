using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace NI2S.Node.Network.Hosting
{
    public static class NetworkHostBuilderExtensions
    {
        public static IHostBuilder ConfigureNetworkingServices(this IHostBuilder hostBuilder, Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            return hostBuilder;
        }
    }
}
