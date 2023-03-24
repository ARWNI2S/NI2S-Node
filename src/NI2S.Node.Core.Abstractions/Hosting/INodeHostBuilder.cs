using Microsoft.Extensions.Hosting;

namespace NI2S.Node.Hosting
{
    /// <summary>
    /// Node program initialization abstraction.
    /// </summary>
    public interface INodeHostBuilder : IHostBuilder
    {
        IEngineContext EngineContext { get; }
    }
}
