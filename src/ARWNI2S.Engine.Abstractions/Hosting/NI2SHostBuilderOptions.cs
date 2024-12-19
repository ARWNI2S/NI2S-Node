namespace ARWNI2S.Engine.Hosting
{
    /// <summary>
    /// Builder options for use with ConfigureNI2SHost.
    /// </summary>
    public class NI2SHostBuilderOptions
    {
        /// <summary>
        /// Indicates if "ARWNI2S_" prefixed environment variables should be added to configuration.
        /// They are added by default.
        /// </summary>
        public bool SuppressEnvironmentConfiguration { get; set; }
    }
}
