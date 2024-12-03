using Microsoft.Extensions.Configuration;

namespace ARWNI2S.Node.Hosting.Builder
{
    /// <summary>
    /// Context containing the common services on the <see cref="INodeHost" />. Some properties may be null until set by the <see cref="INodeHost" />.
    /// </summary>
    public class NodeHostBuilderContext
    {
        /// <summary>
        /// The <see cref="INodeHostEnvironment" /> initialized by the <see cref="INodeHost" />.
        /// </summary>
        public INodeHostEnvironment HostingEnvironment { get; set; } = default!;

        /// <summary>
        /// The <see cref="IConfiguration" /> containing the merged configuration of the application and the <see cref="INodeHost" />.
        /// </summary>
        public IConfiguration Configuration { get; set; } = default!;
    }
}