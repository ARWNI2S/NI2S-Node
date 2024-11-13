namespace ARWNI2S.Node.Configuration.Options
{
    /// <summary>
    /// Builder options for use with ConfigureNodeHost.
    /// </summary>
    public class NodeHostBuilderOptions
    {
        /// <summary>
        /// Indicates if "ARWNI2S_" prefixed environment variables should be added to configuration.
        /// They are added by default.
        /// </summary>
        public bool SuppressEnvironmentConfiguration { get; set; }
    }
}