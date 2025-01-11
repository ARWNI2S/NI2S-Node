// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S
{
    /// <summary>
    /// Represents NI2S version
    /// </summary>
    public static class NI2SVersion
    {
        /// <summary>
        /// Gets the major framework version
        /// </summary>
        public const string CURRENT_VERSION = "0.2";

        /// <summary>
        /// Gets the minor framework version
        /// </summary>
        public const string MINOR_VERSION = "101";

#if DEBUG
        /// <summary>
        /// Gets the framework version tag
        /// </summary>
        public const string VERSION_TAG = "debug";
#endif

        /// <summary>
        /// Gets the full framework version
        /// </summary>
#if DEBUG
        public const string FULL_VERSION = CURRENT_VERSION + "." + MINOR_VERSION + "-" + VERSION_TAG;
#else
        public const string FULL_VERSION = CURRENT_VERSION + "." + MINOR_VERSION;
#endif
    }
}
