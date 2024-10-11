using ARWNI2S.Node.Data.Entities.Directory;
using ARWNI2S.Node.Data.Services.Plugins;

namespace ARWNI2S.Node.Data.Services.Directory
{
    /// <summary>
    /// Exchange rate provider interface
    /// </summary>
    public partial interface IExchangeRateProvider : IModule
    {
        string ProviderBase { get; }

        /// <summary>
        /// Gets currency live rates
        /// </summary>
        /// <param name="exchangeRateCurrencyCode">Exchange rate currency code</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the exchange rates
        /// </returns>
        Task<IList<ExchangeRate>> GetCurrencyLiveRatesAsync(string exchangeRateCurrencyCode);

        /// <summary>
        /// Gets token live rates
        /// </summary>
        /// <param name="exchangeRateTokenCode">Exchange rate token code</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the exchange rates
        /// </returns>
        Task<IList<ExchangeRate>> GetTokenLiveRatesAsync(string exchangeRateTokenCode);
    }
}