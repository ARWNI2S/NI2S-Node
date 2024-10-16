using ARWNI2S.Node.Data.Entities.Users;
using ARWNI2S.Node.Services.Plugins;

namespace ARWNI2S.Node.Services.Directory
{
    /// <summary>
    /// Represents an exchange rate module manager
    /// </summary>
    public partial interface IExchangeRateModuleManager : IModuleManager<IExchangeRateProvider>
    {
        /// <summary>
        /// Load primary active currency exchange rate provider
        /// </summary>
        /// <param name="user">Filter by user; pass null to load all modules</param>
        /// <param name="serverId">Filter by server; pass 0 to load all modules</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the exchange rate provider
        /// </returns>
        Task<IExchangeRateProvider> LoadCurrencyModuleAsync(User user = null, int serverId = 0);

        /// <summary>
        /// Check whether the passed exchange rate provider is active
        /// </summary>
        /// <param name="exchangeRateProvider">Exchange rate provider to check</param>
        /// <returns>Result</returns>
        bool IsModuleActive(IExchangeRateProvider exchangeRateProvider);
    }
}