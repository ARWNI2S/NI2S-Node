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

    /// <summary>
    /// Represents the base interface for entities
    /// </summary>
    public interface IEntityProxy : IEntity
    {

    }
}
