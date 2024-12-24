namespace ARWNI2S.Hosting.Configuration
{
    /// <summary>
    /// Options for configuring the behavior for <see cref="NI2SHost.CreateBuilder(NI2SNodeOptions)"/>.
    /// </summary>
    public class NI2SNodeOptions
    {
        /// <summary>
        /// The command line arguments.
        /// </summary>
        public string[] Args { get; init; }

        /// <summary>
        /// The environment name.
        /// </summary>
        public string EnvironmentName { get; init; }

        /// <summary>
        /// The application name.
        /// </summary>
        public string ApplicationName { get; init; }

        /// <summary>
        /// The content root path.
        /// </summary>
        public string ContentRootPath { get; init; }

        /// <summary>
        /// The web root path.
        /// </summary>
        public string NI2SRootPath { get; init; }
    }
}
