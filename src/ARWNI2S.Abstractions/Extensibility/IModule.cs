
using ARWNI2S.Lifecycle;
using Orleans;

namespace ARWNI2S.Extensibility
{
    public interface IModule : IConfigureEngine, IDescriptor
    {
        IList<string> ModuleDependencies { get; }
    }

    public interface IEngineModule : IModule, ILifecycleParticipant<IEngineLifecycle>
    {
    }
}