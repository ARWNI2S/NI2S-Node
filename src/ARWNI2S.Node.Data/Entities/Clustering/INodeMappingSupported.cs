namespace ARWNI2S.Node.Data.Entities.Clustering
{
    /// <summary>
    /// Represents an entity which supports server mapping
    /// </summary>
    public partial interface INodeMappingSupported
    {
        /// <summary>
        /// Gets or sets a value indicating whether the entity is limited/restricted to certain servers
        /// </summary>
        bool LimitedToServers { get; set; }
    }
}
