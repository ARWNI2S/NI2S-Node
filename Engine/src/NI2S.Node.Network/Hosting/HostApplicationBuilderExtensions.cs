using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace NI2S.Node.Network.Hosting
{
    public static class NetworkHostApplicationBuilderExtensions
    {
        public static HostApplicationBuilder ConfigureNetworkingServices(this HostApplicationBuilder builder, Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            return builder;
        }
    }
}
