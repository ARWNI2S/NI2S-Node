namespace ARWNI2S.Engine.Core
{
    public abstract class EntityBase : INiisEntity
    {
        internal virtual EntityId Id { get; set; }

        object INiisEntity.Id => Id;
    }
}
