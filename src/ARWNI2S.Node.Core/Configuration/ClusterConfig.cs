using ARWNI2S.Infrastructure.Configuration;

namespace ARWNI2S.Node.Core.Configuration
{
    /// <summary>
    /// Represents cluster configuration parameters
    /// </summary>
    public partial class ClusterConfig : IConfig
    {
        /// <summary>
        /// Gets or sets a value indicating whether to use frontline nodes as proxy and load balancers
        /// </summary>
        public bool UseFrontline { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable editor node accessor.
        /// </summary>
        public bool EnableEditor { get; private set; }

        /// <summary>
        /// Gets or sets a value to auto scale cluster.
        /// </summary>
        public bool AutoScale { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating if is a development cluster.
        /// </summary>
        public bool IsDevelopment { get; private set; }

        public string ClusterId { get; set; } = "ni2s-primary";

        public string ServiceId { get; set; } = "ni2s";

        /// <summary>
        /// Gets or sets addresses of known nodes to connect with
        /// </summary>
        public string KnownNodes { get; private set; } = string.Empty;

    }
}