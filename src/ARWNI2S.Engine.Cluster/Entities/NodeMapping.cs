﻿using ARWNI2S.Engine.Data;

namespace ARWNI2S.Cluster.Entities
{
    /// <summary>
    /// Represents a node mapping record
    /// </summary>
    public partial class NodeMapping : DataEntity
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