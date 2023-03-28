using NI2S.Node.Runtime;
using NI2S.Node.Runtime.Identity;

namespace NI2S.Node
{
    /// <summary>
    /// Internal interface implemented by the SystemTarget base class that enables generation of grain references for system targets.
    /// </summary>
    internal interface ISystemTargetBase : IGrainContext
    {
        /// <summary>
        /// Gets the address of the server which this system target is activated on.
        /// </summary>
        SiloAddress Silo { get; }
    }
}