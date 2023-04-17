// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

namespace NI2S.Node.Engine
{
    public enum IncompatibleType
    {
        /// <summary>
        /// The plugin isn't compatible with the current version
        /// </summary>
        NotCompatibleWithCurrentVersion,
        /// <summary>
        /// The main assembly isn't found
        /// </summary>
        MainAssemblyNotFound,
        /// <summary>
        /// Another version of the same library is already loaded in memory
        /// </summary>
        HasCollisions
    }
}
