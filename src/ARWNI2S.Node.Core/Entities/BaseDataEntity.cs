namespace ARWNI2S.Node.Core.Entities
{
    /// <summary>
    /// Represents the base class for entities
    /// </summary>
    public abstract partial class BaseDataEntity : BaseEntity, INodeEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public new int Id { get; set; }

        object INodeEntity.Id => Id;
    }
}
