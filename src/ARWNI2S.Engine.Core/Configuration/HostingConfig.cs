using ARWNI2S.Configuration;

namespace ARWNI2S.Core.Configuration
{
    /// <summary>
    /// Represents hosting configuration parameters
    /// </summary>
    public partial class HostingConfig : IConfig
    {
        /// <summary>
        /// Gets or sets a value indicating whether to use proxy servers and load balancers
        /// </summary>
        public bool UseProxy { get; private set; }

        /// <summary>
        /// Gets or sets addresses of known nodes to connect with
        /// </summary>
        public string KnownNodes { get; private set; } = string.Empty;
    }
}