// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.Configuration;

namespace NI2S.Node.Hosting.Builder
{
    /// <summary>
    /// Context containing the common services on the <see cref="INodeHost" />. Some properties may be null until set by the <see cref="INodeHost" />.
    /// </summary>
    public class NodeHostBuilderContext
    {
        /// <summary>
        /// The <see cref="INodeHostEnvironment" /> initialized by the <see cref="INodeHost" />.
        /// </summary>
        public INodeHostEnvironment HostingEnvironment { get; set; }

        /// <summary>
        /// The <see cref="IConfiguration" /> containing the merged configuration of the engine and the <see cref="INodeHost" />.
        /// </summary>
        public IConfiguration Configuration { get; set; }
    }
}