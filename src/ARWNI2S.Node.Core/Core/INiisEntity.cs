namespace ARWNI2S.Node.Core
{
    /// <summary>
    /// Represents the base interface for entities
    /// </summary>
    public interface INiisEntity : IEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public Guid UUID { get; }

        object IEntity.Id => UUID;
    }
}
