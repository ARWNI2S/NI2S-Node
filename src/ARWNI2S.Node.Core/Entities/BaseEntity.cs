using ARWNI2S.Infrastructure.Entities;

namespace ARWNI2S.Node.Core.Entities
{
    /// <summary>
    /// Represents the base class for entities
    /// </summary>
    public abstract partial class BaseEntity : INI2SEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public object Id { get; set; }
    }
}
