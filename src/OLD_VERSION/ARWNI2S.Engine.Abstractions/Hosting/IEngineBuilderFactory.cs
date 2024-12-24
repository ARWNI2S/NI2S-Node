using ARWNI2S.Engine.Extensibility;

namespace ARWNI2S.Engine.Hosting
{
    public interface IEngineBuilderFactory
    {
        IEngineBuilder CreateBuilder(IModuleCollection modules);
    }
}