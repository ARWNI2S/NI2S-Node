namespace ARWNI2S.Clustering.Nodes.Configuration
{
    /// <summary>
    /// Represents installation configuration parameters
    /// </summary>
    public partial class DeploymentConfig : IConfig
    {

        /// <summary>
        /// Gets or sets a list of plugins ignored during NI2S installation
        /// </summary>
        public string DisabledPlugins { get; private set; } = "*";

        /// <summary>
        /// Gets or sets a value indicating whether to download and setup the regional language pack during installation
        /// </summary>
        public bool InstallRegionalResources { get; private set; } = true;

        /// <inheritdoc/>
        public int GetOrder() => 3;
    }
}