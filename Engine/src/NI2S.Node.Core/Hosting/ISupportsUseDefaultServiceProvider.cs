using Microsoft.Extensions.DependencyInjection;
using System;

namespace NI2S.Node.Hosting
{
    internal interface ISupportsUseDefaultServiceProvider
    {
        INodeHostBuilder UseDefaultServiceProvider(Action<NodeHostBuilderContext, ServiceProviderOptions> configure);
    }
}