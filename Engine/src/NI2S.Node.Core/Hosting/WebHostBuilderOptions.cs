namespace NI2S.Node.Hosting
{
    /// <summary>
    /// Builder options for use with ConfigureWebHost.
    /// </summary>
    public class WebHostBuilderOptions
    {
        /// <summary>
        /// Indicates if "ASPNETCORE_" prefixed environment variables should be added to configuration.
        /// They are added by default.
        /// </summary>
        public bool SuppressEnvironmentConfiguration { get; set; }
    }
}