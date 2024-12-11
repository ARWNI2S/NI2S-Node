using ARWNI2S.Engine.Features;

namespace ARWNI2S.Clustering
{
    public interface INiisRelay : INiisNode
    {
        IFeatureCollection Features { get; }
    }
}
