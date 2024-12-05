using ARWNI2S.Infrastructure.Engine;

namespace ARWNI2S.Engine.Features
{
    internal interface IEngineModulesFeature
    {
        List<IEngineModule> Modules { get; }
    }
}