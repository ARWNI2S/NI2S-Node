namespace NI2S.Node.Configuration.Options
{
    /// <summary>
    /// Represents a set of command line options.
    /// </summary>
    public interface ICommandLineOptions
    {
        /// <summary>
        /// The command line arguments.
        /// </summary>
        string[]? Args { get; init; }
    }
}