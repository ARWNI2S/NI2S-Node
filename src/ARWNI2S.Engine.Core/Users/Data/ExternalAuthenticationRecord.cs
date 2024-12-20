﻿using ARWNI2S.Data.Entities;

namespace ARWNI2S.Engine.Users.Data
{
    /// <summary>
    /// Represents an external authentication record
    /// </summary>
    public partial class ExternalAuthenticationRecord : DataEntity
    {
        /// <summary>
        /// Gets or sets the user identifier
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the external email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the external identifier
        /// </summary>
        public string ExternalIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the external display identifier
        /// </summary>
        public string ExternalDisplayIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the OAuthToken
        /// </summary>
        public string OAuthToken { get; set; }

        /// <summary>
        /// Gets or sets the OAuthAccessToken
        /// </summary>
        public string OAuthAccessToken { get; set; }

        /// <summary>
        /// Gets or sets the provider
        /// </summary>
        public string ProviderSystemName { get; set; }
    }
}
