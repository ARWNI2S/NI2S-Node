namespace ARWNI2S.Node.Core.Network
{
    public interface INodeClientFactory
    {
        NodeClient GetOrCreateClient<TScope>();
 }
}