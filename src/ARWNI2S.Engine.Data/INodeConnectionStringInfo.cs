// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Engine.Data
{
    /// <summary>
    /// Represents a connection string info
    /// </summary>
    public interface INodeConnectionStringInfo
    {
        /// <summary>
        /// DatabaseName
        /// </summary>
        string DatabaseName { get; set; }

        /// <summary>
        /// Server name or IP address
        /// </summary>
        string ServerName { get; set; }

        /// <summary>
        /// Integrated security
        /// </summary>
        bool IntegratedSecurity { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        string Password { get; set; }
    }
}
