namespace ARWNI2S.Infrastructure
{
    /// <summary>
    /// Represents Metalink version
    /// </summary>
    public static class NI2SVersion
    {
        /// <summary>
        /// Gets the major server version
        /// </summary>
        public const string CURRENT_VERSION = "1.0";

        /// <summary>
        /// Gets the minor server version
        /// </summary>
        public const string MINOR_VERSION = "001";

#if DEBUG
        /// <summary>
        /// Gets the server version tag
        /// </summary>
        public const string VERSION_TAG = "debug";
#endif

        /// <summary>
        /// Gets the full server version
        /// </summary>
#if DEBUG
        public const string FULL_VERSION = CURRENT_VERSION + "." + MINOR_VERSION + "-" + VERSION_TAG;
#else
        public const string FULL_VERSION = CURRENT_VERSION + "." + MINOR_VERSION;
#endif
    }
}
