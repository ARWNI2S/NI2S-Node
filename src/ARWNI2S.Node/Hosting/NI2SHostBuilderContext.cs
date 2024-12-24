using Microsoft.Extensions.Configuration;

namespace ARWNI2S.Node.Hosting
{
    /// <summary>
    /// Context containing the common services on the <see cref="INiisHost" />. Some properties may be null until set by the <see cref="INiisHost" />.
    /// </summary>
    public class NI2SHostBuilderContext
    {
        /// <summary>
        /// The <see cref="INiisHostEnvironment" /> initialized by the <see cref="INiisHost" />.
        /// </summary>
        public INiisHostEnvironment HostingEnvironment { get; set; } = default!;

        /// <summary>
        /// The <see cref="IConfiguration" /> containing the merged configuration of the application and the <see cref="INiisHost" />.
        /// </summary>
        public IConfiguration Configuration { get; set; } = default!;
    }
}