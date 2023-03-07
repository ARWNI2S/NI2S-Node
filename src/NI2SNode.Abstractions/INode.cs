namespace NI2S.Node
{
    public interface INode : INodeInfo, IDisposable, IAsyncDisposable
    {
        Task<bool> StartAsync();

        Task StopAsync();
    }
}