using ARWNI2S.Engine.Features;

namespace ARWNI2S.Engine.Builder
{
    public interface IEngineBuilderFactory
    {
        IEngineBuilder CreateBuilder(IFeatureCollection features);
    }
}
