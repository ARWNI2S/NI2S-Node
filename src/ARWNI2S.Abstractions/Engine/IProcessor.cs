using ARWNI2S.Lifecycle;
using Orleans;

namespace ARWNI2S.Engine
{
    public interface IProcessor : ILifecycleParticipant<IEngineLifecycle>
    {
    }
}
