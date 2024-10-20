﻿using ARWNI2S.Infrastructure.Configuration;

namespace ARWNI2S.Node.Core.Entities.Security
{
    /// <summary>
    /// Security settings
    /// </summary>
    public partial class SecuritySettings : ISettings
    {
        /// <summary>
        /// Gets or sets an encryption key
        /// </summary>
        public string EncryptionKey { get; set; }

        /// <summary>
        /// Gets or sets a list of admin allowed IP addresses
        /// </summary>
        public List<string> AdminAllowedIpAddresses { get; set; }
    }
}