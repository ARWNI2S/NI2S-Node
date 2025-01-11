using ARWNI2S.Lifecycle;
using Orleans;

namespace ARWNI2S.Extensibility
{
    public interface IModule : IDescriptor
    {
        IList<string> ModuleDependencies { get; }
    }

    public interface IBuildModuleSupported
    {
        IModuleDataSource DataSource { get; }
    }

    public interface IEngineModule : IModule, IBuildModuleSupported, ILifecycleParticipant<IEngineLifecycle>
    {
    }
}