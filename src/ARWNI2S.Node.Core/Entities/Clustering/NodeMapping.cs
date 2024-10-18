namespace ARWNI2S.Node.Core.Entities.Clustering
{
    /// <summary>
    /// Represents a node mapping record
    /// </summary>
    public partial class NodeMapping : BaseEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// Gets or sets the entity name
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Gets or sets the node identifier
        /// </summary>
        public int NodeId { get; set; }
    }
}
