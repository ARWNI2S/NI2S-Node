namespace ARWNI2S.Hosting
{
    internal class NI2SHostDefaults
    {
        /// <summary>
        /// The configuration key associated with an application name.
        /// </summary>
        public static readonly string ApplicationKey = "applicationName";

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
        public static readonly string NodeRootKey = "root";

        /// <summary>
        /// The configuration key associated with the "captureStartupErrors" configuration.
        /// </summary>
        public static readonly string CaptureStartupErrorsKey = "captureStartupErrors";

        /// <summary>
        /// The configuration key associated with the "urls" configuration.
        /// </summary>
        public static readonly string ServerUrlsKey = "urls";

        /// <summary>
        /// The configuration key associated with the "http_ports" configuration.
        /// </summary>
        public static readonly string HttpPortsKey = "http_ports";

        /// <summary>
        /// The configuration key associated with the "https_ports" configuration.
        /// </summary>
        public static readonly string HttpsPortsKey = "https_ports";

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
        /// The configuration key associated with the "LocalAssets" configuration.
        /// </summary>
        public static readonly string LocalAssetsKey = "staticNI2SAssets";
    }
}