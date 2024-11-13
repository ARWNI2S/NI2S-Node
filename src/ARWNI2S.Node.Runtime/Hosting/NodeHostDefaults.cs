namespace ARWNI2S.Node.Hosting
{
    public static class NodeHostDefaults
    {
        public static readonly string EngineKey = "engineName";
        public static readonly string StartupAssemblyKey = "startupAssembly";
        public static readonly string HostingStartupAssembliesKey = "hostingStartupAssemblies";
        public static readonly string HostingStartupExcludeAssembliesKey = "hostingStartupExcludeAssemblies";

        public static readonly string DetailedErrorsKey = "detailedErrors";
        public static readonly string EnvironmentKey = "environment";
        public static readonly string NodeRootKey = "noderoot";
        public static readonly string CaptureStartupErrorsKey = "captureStartupErrors";
        public static readonly string ServerUrlsKey = "urls";
        public static readonly string ContentRootKey = "contentRoot";
        public static readonly string PreferHostingUrlsKey = "preferHostingUrls";
        public static readonly string PreventHostingStartupKey = "preventHostingStartup";
        public static readonly string SuppressStatusMessagesKey = "suppressStatusMessages";

        public static readonly string ShutdownTimeoutKey = "shutdownTimeoutSeconds";

        public static readonly string StaticNodeAssetsKey = "staticNodeAssets";
    }
}