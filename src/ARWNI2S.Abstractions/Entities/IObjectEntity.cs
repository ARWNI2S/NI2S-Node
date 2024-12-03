namespace ARWNI2S.Entities
{
    /// <summary>
    /// Represents the base interface for every NI2S runtime object
    /// </summary>
    public interface IObjectEntity : IEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        Guid UUID { get; }

        object IEntity.Id => UUID;
    }
}
