﻿namespace ARWNI2S.Node.Core.Entities.Clustering
{
    public enum NodeState : byte
    {
        Offline = 0,
        Online = 1,
        Joining = 10,
        Leaving = 11,

        Error = 255
    }

    /// <summary>
    /// Represents a node
    /// </summary>
    public partial class NI2SNode : BaseEntity
    {
        public string Name { get; set; }

        public Guid NodeId { get; set; }

        public string Metadata { get; set; }
        public string IpAddress { get; set; }
        public string PublicPort { get; set; }
        public string RelayPort { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether SSL is enabled
        /// </summary>
        public bool SslEnabled { get; set; }

        /// <summary>
        /// Gets or sets the comma separated list of possible HTTP_HOST values
        /// </summary>
        public string Hosts { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the default language for this node; 0 is set when we use the default language display order
        /// </summary>
        public int DefaultLanguageId { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        public NodeState CurrentState { get; set; }
        public int AverageEntities { get; set; }
        public int MaxEntities { get; set; }
    }
}
