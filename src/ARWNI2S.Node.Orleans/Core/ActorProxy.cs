using ARWNI2S.Core;
using Orleans;

namespace ARWNI2S.Node.Core
{
    public class ActorProxy : Grain<PersistentState>, INiisActorProxy
    {
    }
}
