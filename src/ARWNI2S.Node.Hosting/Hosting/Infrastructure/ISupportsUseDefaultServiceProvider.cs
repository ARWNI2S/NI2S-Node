using ARWNI2S.Node.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Node.Hosting.Infrastructure
{
    internal interface ISupportsUseDefaultServiceProvider
    {
        INodeHostBuilder UseDefaultServiceProvider(Action<NodeHostBuilderContext, ServiceProviderOptions> configure);
    }
}