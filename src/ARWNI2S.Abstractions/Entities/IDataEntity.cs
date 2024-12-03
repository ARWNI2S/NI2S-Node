namespace ARWNI2S.Entities
{
    /// <summary>
    /// Represents the base interface for every data entity model
    /// </summary>
    public interface IDataEntity : IEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        new int Id { get; set; }

        object IEntity.Id => Id;
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
