using ARWNI2S.Engine.Actor;

namespace ARWNI2S.Engine.Components
{
    public abstract class ActorComponent : NI2SObject, IActorComponent
    {
        public NI2SActor Owner { get; private set; }

        IActorEntity IActorComponent.Owner => Owner;
    }
}