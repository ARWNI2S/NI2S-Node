namespace ARWNI2S.Engine.Core
{
    public abstract class EntityBase : INiisEntity
    {
        internal virtual EntityId EntityId { get; }

        object INiisEntity.Id => EntityId;
    }
}
