namespace ARWNI2S.Entities
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
