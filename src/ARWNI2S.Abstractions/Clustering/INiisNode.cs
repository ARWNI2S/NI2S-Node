using ARWNI2S.Entities;

namespace ARWNI2S.Clustering
{
    public interface INiisNode : IDataEntity
    {
        public Guid NodeId { get; }
    }
}
