﻿namespace ARWNI2S.Entities
{
    public abstract class NI2SActor : NI2SObject, IActorEntity
    {
        private IActorGrain _grainReference;

        protected IActorGrain Self => _grainReference;

        public ActorComponents Components { get; } = new ActorComponents();

        ICollection<IActorComponent> IActorEntity.Components => Components;
    }

    public abstract class NI2SActor<TGrain> : NI2SActor
        where TGrain : IActorGrain
    {
        protected new TGrain Self => (TGrain)base.Self;
    }
}
