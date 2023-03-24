using Microsoft.Extensions.Hosting;

namespace NI2S.Node.Hosting
{
    /// <summary>
    /// Node program abstraction.
    /// </summary>
    public interface INodeHost : IHost
    {
        IEngineContext EngineContext { get; }
    }
}
