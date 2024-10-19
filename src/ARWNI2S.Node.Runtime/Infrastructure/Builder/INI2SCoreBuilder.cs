using ARWNI2S.Runtime.EngineParts;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Runtime.Infrastructure.Builder
{
    public interface INI2SCoreBuilder
    {
        IServiceCollection Services { get; }
        EnginePartManager PartManager { get; }
    }
}