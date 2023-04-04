// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

namespace NI2S.Node.Core
{
    /// <summary>
    /// Represents descriptor of the node engine components (plugin or module)
    /// </summary>
    public partial interface IDescriptor
    {
        /// <summary>
        /// Gets or sets the system name
        /// </summary>
        string SystemName { get; }

        /// <summary>
        /// Gets or sets the friendly name
        /// </summary>
        string FriendlyName { get; }
    }
}
