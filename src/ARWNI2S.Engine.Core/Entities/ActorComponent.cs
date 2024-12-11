
namespace ARWNI2S.Entities
{
    public abstract class ActorComponent : NI2SObject, IActorComponent
    {
        public NI2SActor Owner {  get; private set; }

        IActorEntity IActorComponent.Owner => Owner;
    }
}