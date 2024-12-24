using ARWNI2S.Engine;
using ARWNI2S.Engine.Features;

namespace ARWNI2S.Clustering
{
    public interface IClusterServer : IDisposable
    {
        Guid NodeId { get; }
        INiisNode Node { get; }

        IFeatureCollection Features { get; }

        Task StartAsync(INiisEngine ni2sEngine, CancellationToken cancellationToken);

        Task StopAsync(CancellationToken cancellationToken);
    }
}
