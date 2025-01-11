// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Engine.Collections;
using ARWNI2S.Engine.Core.Object;

namespace ARWNI2S.Engine.Core.Actor
{
    public abstract class NI2SActor : NI2SObject, INiisActor
    {
        internal ActorId ActorId { get; }

        protected INiisGrain Self { get; }

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
        internal override EntityId Id => ActorId.EntityId;
        object INiisEntity.Id => ActorId;

        IEnumerable<IActorComponent> INiisActor.Components => Components;
    }
}
