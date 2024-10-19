using ARWNI2S.Runtime.Core.Components;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Runtime.Core.Builder
{
    public interface INI2SCoreBuilder
    {
        IServiceCollection Services { get; }
        EnginePartManager PartManager { get; }
    }
}