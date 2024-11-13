namespace ARWNI2S.Node.Hosting
{
    //
    // Resumen:
    //     Represents a configured node host.
    public interface INodeHost : IDisposable
    {
        //IFeatureCollection ServerFeatures { get; }
        IServiceProvider Services { get; }

        void Start();
        Task StartAsync(CancellationToken cancellationToken = default);
        Task StopAsync(CancellationToken cancellationToken = default);
    }
}