namespace ARWNI2S.Infrastructure.Network.Protocol.ProxyProtocol
{
    public interface IProxyProtocolPipelineFilter : IPipelineFilter
    {
        ProxyInfo ProxyInfo { get; }
    }
}