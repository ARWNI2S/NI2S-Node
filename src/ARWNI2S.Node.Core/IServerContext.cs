using ARWNI2S.Node.Core.Entities.Clustering;

namespace ARWNI2S.Node.Core
{
    /// <summary>
    /// Server context
    /// </summary>
    public interface IServerContext
    {
        /// <summary>
        /// Gets the current server
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task<IBladeServer> GetCurrentServerAsync();

        /// <summary>
        /// Gets the current server
        /// </summary>
        IBladeServer GetCurrentServer();

        /// <summary>
        /// Gets active server scope configuration
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task<int> GetActiveServerScopeConfigurationAsync();
    }
}
