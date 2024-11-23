﻿using ARWNI2S.Node.Configuration;

namespace ARWNI2S.Node.Core.Users
{
    /// <summary>
    /// External authentication settings
    /// </summary>
    public partial class ExternalAuthenticationSettings : ISettings
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ExternalAuthenticationSettings()
        {
            ActiveAuthenticationMethodSystemNames = [];
        }

        /// <summary>
        /// Gets or sets a value indicating whether email validation is required.
        /// In most cases we can skip email validation for Facebook or any other third-party external authentication modules. I guess we can trust  Facebook for the validation.
        /// </summary>
        public bool RequireEmailValidation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether need to logging errors on authentication process 
        /// </summary>
        public bool LogErrors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is allowed to remove external authentication associations
        /// </summary>
        public bool AllowUsersToRemoveAssociations { get; set; }

        /// <summary>
        /// Gets or sets system names of active authentication methods
        /// </summary>
        public List<string> ActiveAuthenticationMethodSystemNames { get; set; }
    }
}
