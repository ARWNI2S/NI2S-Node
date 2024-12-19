using ARWNI2S.Engine;
using ARWNI2S.Engine.Extensibility;

namespace ARWNI2S.Node.Clustering
{
    public interface IClusterNode
    {
        IModuleCollection Modules { get; }

        Task StartAsync(INiisEngine engine, CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
    }
}
