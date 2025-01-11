namespace ARWNI2S.Cluster
{
    public interface INodeContext
    {
        Task<INiisNode> GetCurrentNodeAsync();
    }
}
