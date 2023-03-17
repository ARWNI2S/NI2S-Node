namespace NI2S.Node.Hosting
{
    /// <summary>
    /// Minimal host builder interface.
    /// </summary>
    public interface IMinimalApiHostBuilder
    {
        /// <summary>
        /// Configures default services for a minimal api.
        /// </summary>
        void ConfigureHostBuilder();
    }
}
