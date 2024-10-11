using Microsoft.Extensions.Configuration;

namespace ARWNI2S.Node.Hosting
{
    /// <summary>
    /// Context containing the common services on the <see cref="INI2SHost" />. Some properties may be null until set by the <see cref="INI2SHost" />.
    /// </summary>
    public class NI2SHostBuilderContext
    {
        /// <summary>
        /// The <see cref="INI2SHostEnvironment" /> initialized by the <see cref="INI2SHost" />.
        /// </summary>
        public INI2SHostEnvironment HostingEnvironment { get; set; } = default!;

        /// <summary>
        /// The <see cref="IConfiguration" /> containing the merged configuration of the application and the <see cref="INI2SHost" />.
        /// </summary>
        public IConfiguration Configuration { get; set; } = default!;
    }
}
