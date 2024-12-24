﻿using ARWNI2S.Data.Entities;

namespace ARWNI2S.Engine.Security.Data
{
    /// <summary>
    /// Represents a permission record
    /// </summary>
    public partial class PermissionRecord : DataEntity
    {
        /// <summary>
        /// Gets or sets the permission name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the permission system name
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the permission category
        /// </summary>
        public string Category { get; set; }
    }
}