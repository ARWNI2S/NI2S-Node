namespace ARWNI2S.Infrastructure.Entities
{
    /// <summary>
    /// Represents the base interface for entities
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets the entity id object
        /// </summary>
        object Id { get; }
    }

    public interface IDataEntity : IEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        new int Id { get; set; }

        object IEntity.Id => Id;
    }

    /// <summary>
    /// Represents the base interface for entities
    /// </summary>
    public interface IObjectEntity : IEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public Guid UUID { get; }

        object IEntity.Id => UUID;
    }
}
