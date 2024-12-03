using ARWNI2S.Core.Actor;
using ARWNI2S.Core.Object;
using ARWNI2S.Entities;

namespace ARWNI2S.Core.Entities
{
    public abstract class NiisActor : NiisObject, IActorEntity
    {
        private readonly ActorComposition _components = [];

        private IEntityGrain remotingInstance;

        protected virtual IEntityGrain Self => remotingInstance;

        public ActorComposition Components => _components;

        protected NiisActor() : base()
        {

        }

        protected NiisActor(IObjectInitializer objectInitializer) : base(objectInitializer)
        {

        }

        #region IActorEntity implementation

        IEntityGrain IActorEntity.Self => Self;

        IActorComponents IActorEntity.Components => Components;

        #endregion
    }
}
