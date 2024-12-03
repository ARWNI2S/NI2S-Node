namespace ARWNI2S.Node.Hosting
{
    public interface INodeHost : IDisposable
    {
        IFeatureCollection NodeFeatures { get; }
        IServiceProvider Services { get; }

        void Start();
        Task StartAsync(CancellationToken cancellationToken = default);
        Task StopAsync(CancellationToken cancellationToken = default);
    }
}
