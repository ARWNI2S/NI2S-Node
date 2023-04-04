// TODO: find and replace this line with COPYRIGTH NOTICE entire solution

namespace NI2S.Node
{
    /// <summary>
    /// Contains a set of static readonlyants representing configuration keys.
    /// </summary>
    internal static class DefaultConstants
    {
        /// <summary>
        /// The configuration key associated with an node name.
        /// </summary>
        public static readonly string NodeKey = "nodeName";

        /// <summary>
        /// The configuration key associated with the startup assembly.
        /// </summary>
        public static readonly string StartupAssemblyKey = "startupAssembly";

        /// <summary>
        /// The configuration key associated with "hostingStartupAssemblies" configuration.
        /// </summary>
        public static readonly string HostingStartupAssembliesKey = "hostingStartupAssemblies";

        /// <summary>
        /// The configuration key associated with the "hostingStartupExcludeAssemblies" configuration.
        /// </summary>
        public static readonly string HostingStartupExcludeAssembliesKey = "hostingStartupExcludeAssemblies";

        /// <summary>
        /// The configuration key associated with the "DetailedErrors" configuration.
        /// </summary>
        public static readonly string DetailedErrorsKey = "detailedErrors";

        /// <summary>
        /// The configuration key associated with the application's environment setting.
        /// </summary>
        public static readonly string EnvironmentKey = "environment";

        /// <summary>
        /// The configuration key associated with the "webRoot" configuration.
        /// </summary>
        public static readonly string NodeRootKey = "noderoot";

        /// <summary>
        /// The configuration key associated with the "captureStartupErrors" configuration.
        /// </summary>
        public static readonly string CaptureStartupErrorsKey = "captureStartupErrors";

        /// <summary>
        /// The configuration key associated with the "urls" configuration.
        /// </summary>
        public static readonly string NodeAdressesKey = "nodeAddresses";

        /// <summary>
        /// The configuration key associated with the "ContentRoot" configuration.
        /// </summary>
        public static readonly string ContentRootKey = "contentRoot";

        /// <summary>
        /// The configuration key associated with the "PreferHostingUrls" configuration.
        /// </summary>
        public static readonly string PreferHostingUrlsKey = "preferHostingUrls";

        /// <summary>
        /// The configuration key associated with the "PreventHostingStartup" configuration.
        /// </summary>
        public static readonly string PreventHostingStartupKey = "preventHostingStartup";

        /// <summary>
        /// The configuration key associated with the "SuppressStatusMessages" configuration.
        /// </summary>
        public static readonly string SuppressStatusMessagesKey = "suppressStatusMessages";

        /// <summary>
        /// The configuration key associated with the "ShutdownTimeoutSeconds" configuration.
        /// </summary>
        public static readonly string ShutdownTimeoutKey = "shutdownTimeoutSeconds";

        /// <summary>
        /// The configuration key associated with the "StaticNodeAssets" configuration.
        /// </summary>
        public static readonly string StaticNodeAssetsKey = "staticNodeAssets";
    }
}