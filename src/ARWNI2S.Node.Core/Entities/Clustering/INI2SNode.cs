using ARWNI2S.Infrastructure.Entities;

namespace ARWNI2S.Node.Core.Entities.Clustering
{
    /// <summary>
    /// Represents a NI2S node
    /// </summary>
    public partial interface INI2SNode : INI2SEntity
    {
        /// <summary>
        /// Gets or sets the node id
        /// </summary>
        Guid NodeId { get; set; }

        /// <summary>
        /// Gets or sets the node metadata in simple json format
        /// </summary>
        string Metadata { get; set; }

        /// <summary>
        /// Gets or sets the node IP address
        /// </summary>
        string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the node public port
        /// </summary>
        string PublicPort { get; set; }

        /// <summary>
        /// Gets or sets the node relay port
        /// </summary>
        string RelayPort { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether SSL is enabled
        /// </summary>
        bool SslEnabled { get; set; }

        /// <summary>
        /// Gets or sets the comma separated list of possible HTTP_HOST values
        /// </summary>
        string Hosts { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the default language for this node; 0 is set when we use the default language display order
        /// </summary>
        int DefaultLanguageId { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the current node state
        /// </summary>
        string CurrentState { get; set; }

        /// <summary>
        /// Gets or sets the average number of entities in the node
        /// </summary>
        int AverageEntities { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of entities in the node
        /// </summary>
        int MaxEntities { get; set; }
    }
}
