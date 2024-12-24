namespace ARWNI2S.Engine.Actor
{
    public interface IActorEntity : IObjectEntity
    {
        ICollection<IActorComponent> Components { get; }
    }

    public interface IActorComponent : IObjectEntity
    {
        IActorEntity Owner { get; }
    }
}
