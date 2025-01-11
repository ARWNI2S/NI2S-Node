// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Engine.Core;

namespace ARWNI2S.Engine.Data.Events
{
    /// <summary>
    /// A container for entities that have been inserted.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class EntityCreatedEvent<T> where T : INiisEntity
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="entity">Entity</param>
        public EntityCreatedEvent(T entity)
        {
            Entity = entity;
        }

        /// <summary>
        /// Entity
        /// </summary>
        public T Entity { get; }
    }
}
