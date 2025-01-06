using ARWNI2S.Engine.Collections;
using ARWNI2S.Engine.Core.Object;
using Orleans;

namespace ARWNI2S.Engine.Core.Actor
{
    public abstract class NI2SActor : NI2SObject, INiisActor
    {
        internal ActorId ActorId { get; }

        public INiisGrain Self { get; }

        public ActorComponents Components { get; }

        public Task OnActivateAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        internal override ObjectId ObjectId => ActorId.ObjectId;
        internal override EntityId EntityId => ActorId.EntityId;
        object INiisEntity.Id => ActorId;

        IEnumerable<IActorComponent> INiisActor.Components => Components;
    }
}
