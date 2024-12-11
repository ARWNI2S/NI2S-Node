using ARWNI2S.Engine;

namespace ARWNI2S.Data.Entities
{
    /// <summary>
    /// Represents the base class for entities
    /// </summary>
    public abstract partial class DataEntity : IDataEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public int Id { get; set; }
    }
}
