using ARWNI2S.Infrastructure.Engine;

namespace ARWNI2S.Node.Features
{
    internal interface IEngineModulesFeature
    {
        List<IEngineModule> Modules { get; }
    }
}