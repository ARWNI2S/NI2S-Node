using ARWNI2S.Node.Core;

namespace ARWNI2S.Node.Data.Entities
{
    /// <summary>
    /// Represents the base class for data entities
    /// </summary>
    public abstract partial class DataEntity : IDataEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public int Id { get; set; }
    }
}
