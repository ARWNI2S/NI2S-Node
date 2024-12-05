using ARWNI2S.Engine.Features;

namespace ARWNI2S.Node.Hosting
{
    //
    // Resumen:
    //     Represents a configured node host.
    public interface INodeHost : IDisposable
    {
        IFeatureCollection NodeFeatures { get; }
        IServiceProvider Services { get; }

        void Start();
        Task StartAsync(CancellationToken cancellationToken = default);
        Task StopAsync(CancellationToken cancellationToken = default);
    }
}