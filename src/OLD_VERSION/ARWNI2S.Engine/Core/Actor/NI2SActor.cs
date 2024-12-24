using ARWNI2S.Engine.Core.Object;

namespace ARWNI2S.Engine.Core.Actor
{
    public abstract class NI2SActor : NI2SObject, INiisActor
    {
        internal ActorComponents Components { get; }

        IEnumerable<IActorComponent> INiisActor.Components => Components;


    }
}