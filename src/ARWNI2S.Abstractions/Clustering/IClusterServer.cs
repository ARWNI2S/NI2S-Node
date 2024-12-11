using ARWNI2S.Engine.Features;

namespace ARWNI2S.Clustering
{
    public interface IClusterServer
    {
        Guid NodeId { get; }
        INiisNode Node { get; }

        IFeatureCollection Features { get; }
    }
}
