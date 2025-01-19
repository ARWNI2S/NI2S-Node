using ARWNI2S.Core;
using Orleans;

namespace ARWNI2S.Node.Orleans
{
    internal class ActorProxy : Grain<PersistentState>, INiisActorProxy
    {
    }
}
