// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Data;
using ARWNI2S.Engine.Core;

namespace ARWNI2S.Engine.Data
{
    /// <summary>
    /// Represents the base class for entities
    /// </summary>
    public abstract partial class DataEntity : EntityBase, IDataEntity
    {
        object INiisEntity.Id => Id;

        int IDataEntity.Id { get { return (int)base.Id; } set { base.Id = (EntityId)value; } }
    }

    /// <summary>
    /// Represents a soft-deleted (without actually deleting from storage) entity
    /// </summary>
    public partial interface ISoftDeletedEntity
    {
        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        bool Deleted { get; set; }
    }
}
