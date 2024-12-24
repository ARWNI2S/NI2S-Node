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
        public const string CURRENT_VERSION = "1.0";

        /// <summary>
        /// Gets the minor framework version
        /// </summary>
        public const string MINOR_VERSION = "001";

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
