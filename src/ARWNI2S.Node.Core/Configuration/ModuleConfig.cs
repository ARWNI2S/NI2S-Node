using ARWNI2S.Infrastructure.Configuration;

namespace ARWNI2S.Node.Core.Configuration
{
    /// <summary>
    /// Represents module configuration parameters
    /// </summary>
    public partial class ModuleConfig : IConfig
    {
        /// <summary>
        /// Gets or sets a value indicating whether to load an assembly into the load-from context, bypassing some security checks.
        /// </summary>
        public bool UseUnsafeLoadAssembly { get; set; } = true;

        /// <inheritdoc/>
        public int GetOrder() => 3;
    }
}
