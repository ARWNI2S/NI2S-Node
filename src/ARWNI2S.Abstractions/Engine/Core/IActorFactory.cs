namespace ARWNI2S.Engine.Core
{
    public interface IActorFactory : IObjectFactory
    {
        new TActor CreateInstance<TActor>() where TActor : INiisActor;
        new INiisActor CreateInstance(Type type);
    }

    public interface IActorFactory<TActor> : IObjectFactory<TActor> where TActor : INiisActor
    {
        new TActor CreateInstance();
    }
}