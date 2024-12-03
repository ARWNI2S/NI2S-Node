namespace ARWNI2S.Entities
{
    public interface IActorComponent : IObjectEntity
    {
        IActorEntity Owner { get; }
    }
}