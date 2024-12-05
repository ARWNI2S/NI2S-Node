using ARWNI2S.Node;

namespace ARWNI2S.Engine.Builder
{
    public interface IEngineBuilderFactory
    {
        IEngineBuilder CreateBuilder(IFeatureCollection features);
    }
}
