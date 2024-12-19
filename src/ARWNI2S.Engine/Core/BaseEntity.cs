namespace ARWNI2S.Engine.Core
{
    public abstract class BaseEntity : INiisEntity
    {
        public virtual EntityId Id { get; protected set; }

        object INiisEntity.Id => Id;
    }
}
