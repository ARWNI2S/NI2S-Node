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
