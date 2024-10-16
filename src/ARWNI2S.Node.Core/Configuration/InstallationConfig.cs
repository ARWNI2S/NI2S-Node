using ARWNI2S.Infrastructure.Configuration;

namespace ARWNI2S.Node.Core.Configuration
{
    /// <summary>
    /// Represents installation configuration parameters
    /// </summary>
    public partial class InstallationConfig : IConfig
    {

        /// <summary>
        /// Gets or sets a list of modules ignored during dragonCorp installation
        /// </summary>
        public string DisabledModules { get; private set; } = "*";

        /// <summary>
        /// Gets or sets a value indicating whether to download and setup the regional language pack during installation
        /// </summary>
        public bool InstallRegionalResources { get; private set; } = true;
    }
}