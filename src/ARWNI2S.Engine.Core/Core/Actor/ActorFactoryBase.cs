// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Engine.Core.Actor
{
    public abstract class ActorFactoryBase : IActorFactory
    {
        public abstract TActor CreateInstance<TActor>() where TActor : INiisActor;

        public abstract INiisActor CreateInstance(Type type);

        TObject IObjectFactory.CreateInstance<TObject>() => (TObject)CreateInstance(typeof(TObject));
        INiisObject IObjectFactory.CreateInstance(Type type) => CreateInstance(type);
    }

}
