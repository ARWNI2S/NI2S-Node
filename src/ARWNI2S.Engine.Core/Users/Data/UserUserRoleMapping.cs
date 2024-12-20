﻿using ARWNI2S.Data.Entities;

namespace ARWNI2S.Engine.Users.Data
{
    /// <summary>
    /// Represents a user-user role mapping class
    /// </summary>
    public partial class UserUserRoleMapping : DataEntity
    {
        /// <summary>
        /// Gets or sets the user identifier
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user role identifier
        /// </summary>
        public int UserRoleId { get; set; }
    }
}