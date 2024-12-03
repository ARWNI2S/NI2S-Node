namespace ARWNI2S.Entities
{
    public interface IActorEntity : IObjectEntity
    {
        IEntityGrain Self { get; }

        IActorComponents Components { get; }
    }
}
