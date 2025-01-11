// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Engine.Core.Actor
{
    internal class DefaulActorFactory : ActorFactoryBase, IActorFactory<INiisActor>
    {
        public override TActor CreateInstance<TActor>()
        {
            return (TActor)CreateInstance(typeof(TActor));
        }

        public override INiisActor CreateInstance(Type type)
        {
            return (INiisActor)Activator.CreateInstance(type);
        }

        public INiisActor CreateInstance() { throw new InvalidOperationException($"{nameof(DefaulActorFactory)} should never use {nameof(CreateInstance)} directly."); }

        TObject IObjectFactory.CreateInstance<TObject>() => (TObject)CreateInstance(typeof(TObject));
        INiisObject IObjectFactory.CreateInstance(Type type) => CreateInstance(type);
    }
}
