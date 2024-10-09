namespace ARWNI2S.Node.Core.Entities.Clustering
{
    /// <summary>
    /// Represents a server mapping record
    /// </summary>
    public partial class NodeMapping : BaseDataEntity
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
        /// Gets or sets the server identifier
        /// </summary>
        public int ServerId { get; set; }
    }
}
