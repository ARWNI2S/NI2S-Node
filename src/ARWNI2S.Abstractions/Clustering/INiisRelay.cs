using ARWNI2S.Engine;

namespace ARWNI2S.Clustering
{
    public interface INiisRelay : INiisNode
    {
        IFeatureCollection Features { get; }
    }
}
