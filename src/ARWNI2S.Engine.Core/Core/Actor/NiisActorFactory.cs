using ARWNI2S.Core.Entities;
using ARWNI2S.Core.Object;

namespace ARWNI2S.Core.Actor
{
    internal class NiisActorFactory<TActor> : NiisObjectFactory<TActor>, INiisActorFactory where TActor : NiisActor, new ()
    {
    }
}
