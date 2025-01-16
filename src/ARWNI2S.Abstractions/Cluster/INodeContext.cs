namespace ARWNI2S.Cluster
{
    public interface INodeContext
    {
        INiisNode GetCurrentNode();
        Task<INiisNode> GetCurrentNodeAsync();
    }
}
