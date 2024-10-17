﻿namespace ARWNI2S.Node.Core.Entities.Clustering
{
    /// <summary>
    /// Represents an entity which supports node mapping
    /// </summary>
    public partial interface INodeMappingSupported
    {
        /// <summary>
        /// Gets or sets a value indicating whether the entity is limited/restricted to certain nodes
        /// </summary>
        bool LimitedToNodes { get; set; }
    }
}