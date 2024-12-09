namespace ARWNI2S.Entities
{
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
