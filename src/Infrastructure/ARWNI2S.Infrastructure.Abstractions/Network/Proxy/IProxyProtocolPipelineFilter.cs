using ARWNI2S.Infrastructure.Network.Protocol;

namespace ARWNI2S.Infrastructure.Network.Proxy
{
    public interface IProxyProtocolPipelineFilter : IPipelineFilter
    {
        ProxyInfo ProxyInfo { get; }
    }
}