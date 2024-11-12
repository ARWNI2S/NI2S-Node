using ARWNI2S.Runtime.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Runtime.Hosting.Infrastructure
{
    internal interface ISupportsUseDefaultServiceProvider
    {
        INodeHostBuilder UseDefaultServiceProvider(Action<NodeHostBuilderContext, ServiceProviderOptions> configure);
    }
}