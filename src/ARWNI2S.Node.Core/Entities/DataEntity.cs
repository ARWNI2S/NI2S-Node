using ARWNI2S.Infrastructure.Entities;

namespace ARWNI2S.Node.Core.Entities
{
    /// <summary>
    /// Represents the base class for entities
    /// </summary>
    public abstract partial class DataEntity : IEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public int Id { get; set; }

        object IEntity.Id => Id;
    }
}
