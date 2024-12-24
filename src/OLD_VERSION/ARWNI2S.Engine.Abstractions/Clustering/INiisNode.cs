using ARWNI2S.Engine.Data;

namespace ARWNI2S.Engine.Clustering
{
    public interface INiisNode : IDataEntity
    {
        public Guid NodeId { get; }
    }
}
