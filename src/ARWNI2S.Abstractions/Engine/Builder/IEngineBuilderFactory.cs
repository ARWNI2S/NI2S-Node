using ARWNI2S.Extensibility;

namespace ARWNI2S.Engine.Builder
{
    public interface IEngineBuilderFactory
    {
        IEngineBuilder CreateBuilder(IModuleCollection modules);
    }
}