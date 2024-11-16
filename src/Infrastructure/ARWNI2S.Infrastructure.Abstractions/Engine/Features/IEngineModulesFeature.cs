namespace ARWNI2S.Infrastructure.Engine.Features
{
    internal interface IEngineModulesFeature
    {
        List<IEngineModule> Modules { get; }
    }
}