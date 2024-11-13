using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Node.Core.Engine.Builder
{
    public interface IEngineCoreBuilder
    {
        IServiceCollection Services { get; }

        EnginePartManager PartManager { get; }
    }
}