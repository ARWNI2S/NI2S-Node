namespace ARWNI2S.Node.Hosting.Internal
{
    /// <summary>
    /// Builder options for use with ConfigureNI2SHost.
    /// </summary>
    internal class HostBuilderOptions
    {
        /// <summary>
        /// Indicates if "ARWNI2S_" prefixed environment variables should be added to configuration.
        /// They are added by default.
        /// </summary>
        public bool SuppressEnvironmentConfiguration { get; set; }
    }
}
