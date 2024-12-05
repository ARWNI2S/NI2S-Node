using ARWNI2S.Node.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Node.Hosting.Extensions
{
    internal interface ISupportsUseDefaultServiceProvider
    {
        INodeHostBuilder UseDefaultServiceProvider(Action<NodeHostBuilderContext, ServiceProviderOptions> configure);
    }
}