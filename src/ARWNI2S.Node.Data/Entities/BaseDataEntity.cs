using ARWNI2S.Infrastructure.Entities;
using ARWNI2S.Node.Core.Entities;

namespace ARWNI2S.Node.Data.Entities
{
    /// <summary>
    /// Represents the base class for entities
    /// </summary>
    public abstract partial class BaseDataEntity : BaseEntity, INI2SEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public new int Id { get; set; }

        object INI2SEntity.Id => Id;
    }
}
