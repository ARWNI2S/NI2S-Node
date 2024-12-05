using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Engine.Builder
{
    public interface INiisBuilder
    {
        IServiceCollection Services { get; }

        IEnginePartManager PartManager { get; }
    }
}