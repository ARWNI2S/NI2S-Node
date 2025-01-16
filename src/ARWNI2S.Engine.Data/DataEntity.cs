using ARWNI2S.Engine.Core;
using ARWNI2S.Framework.Data;

namespace ARWNI2S.Engine.Data
{
    /// <summary>
    /// Represents the base class for entities
    /// </summary>
    public abstract class DataEntity : EntityBase, IDataEntity
    {
        public new int Id { get => base.Id; set => base.Id = (EntityId)value; }
    }

    /// <summary>
    /// Represents a soft-deleted (without actually deleting from storage) entity
    /// </summary>
    public interface ISoftDeletedEntity
    {
        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        bool Deleted { get; set; }
    }
}
