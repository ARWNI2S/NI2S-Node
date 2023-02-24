namespace NI2S.Node.Configuration.Options
{
    /// <summary>
    /// Options for configuring the behavior for <see cref="NodeHost.CreateBuilder(NodeHostOptions)"/>.
    /// </summary>
    public class NodeHostOptions
    {
        /// <summary>
        /// The command line arguments.
        /// </summary>
        public string[]? Args { get; init; }

        /// <summary>
        /// The environment name.
        /// </summary>
        public string? EnvironmentName { get; init; }

        /// <summary>
        /// The application name.
        /// </summary>
        //public string? ApplicationName { get; init; }

        /// <summary>
        /// The content root path.
        /// </summary>
        public string? ContentRootPath { get; init; }

        /// <summary>
        /// The assets root path.
        /// </summary>
        public string? AssetsRootPath { get; init; }
    }
}
