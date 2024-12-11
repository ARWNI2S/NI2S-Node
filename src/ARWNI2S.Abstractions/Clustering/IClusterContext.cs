
namespace ARWNI2S.Clustering
{
    public interface IClusterContext
    {
        Task<INiisNode> GetCurrentNodeAsync();
    }
}
