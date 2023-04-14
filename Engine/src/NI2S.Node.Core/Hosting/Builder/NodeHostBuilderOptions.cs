// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

namespace NI2S.Node.Hosting
{
    /// <summary>
    /// Builder options for use with ConfigureNodeHost.
    /// </summary>
    public class NodeHostBuilderOptions
    {
        /// <summary>
        /// Indicates if "DOTNET_" prefixed environment variables should be added to configuration.
        /// They are added by default.
        /// </summary>
        public bool SuppressEnvironmentConfiguration { get; set; }
    }
}