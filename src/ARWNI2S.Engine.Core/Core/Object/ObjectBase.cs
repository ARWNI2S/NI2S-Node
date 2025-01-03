﻿namespace ARWNI2S.Engine.Core.Object
{
    public abstract class ObjectBase : EntityBase, INiisEntity
    {
        internal virtual ObjectId ObjectId { get; }
        internal override EntityId EntityId => ObjectId.EntityId;
        object INiisEntity.Id => ObjectId;
    }
}
