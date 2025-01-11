using ARWNI2S.Lifecycle;
using Orleans;

namespace ARWNI2S.Engine
{
    public interface INiisProcessor : ILifecycleParticipant<IEngineLifecycle>
    {
    }
}
