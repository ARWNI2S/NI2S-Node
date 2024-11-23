using ARWNI2S.Node.Core.Framework;
using ARWNI2S.Node.Data.Entities.Framework;

namespace ARWNI2S.Node.Services.Framework
{
    /// <summary>
    /// Currency service
    /// </summary>
    public partial interface ICurrencyService
    {
        #region Currency

        /// <summary>
        /// Deletes currency
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteCurrencyAsync(Currency currency);

        /// <summary>
        /// Gets a currency
        /// </summary>
        /// <param name="currencyId">Currency identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the currency
        /// </returns>
        Task<Currency> GetCurrencyByIdAsync(int currencyId);

        /// <summary>
        /// Gets a currency by code
        /// </summary>
        /// <param name="currencyCode">Currency code</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the currency
        /// </returns>
        Task<Currency> GetCurrencyByCodeAsync(string currencyCode);

        /// <summary>
        /// Gets all currencies
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="serverId">Load records allowed only in a specified server; pass 0 to load all records</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the currencies
        /// </returns>
        Task<IList<Currency>> GetAllCurrenciesAsync(bool showHidden = false, int serverId = 0);

        /// <summary>
        /// Inserts a currency
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertCurrencyAsync(Currency currency);

        /// <summary>
        /// Updates the currency
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateCurrencyAsync(Currency currency);

        #endregion

        #region Conversions

        /// <summary>
        /// Gets live rates regarding the passed currency
        /// </summary>
        /// <param name="currencyCode">Currency code; pass null to use primary exchange rate currency</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the exchange rates
        /// </returns>
        Task<IList<ExchangeRate>> GetCurrencyLiveRatesAsync(string currencyCode = null);

        /// <summary>
        /// Converts currency
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="exchangeRate">Currency exchange rate</param>
        /// <returns>Converted value</returns>
        decimal ConvertCurrency(decimal amount, decimal exchangeRate);

        /// <summary>
        /// Converts to primary server currency 
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="sourceCurrencyCode">Source currency code</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the converted value
        /// </returns>
        Task<decimal> ConvertToPrimaryServerCurrencyAsync(decimal amount, Currency sourceCurrencyCode);

        /// <summary>
        /// Converts from primary server currency
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="targetCurrencyCode">Target currency code</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the converted value
        /// </returns>
        Task<decimal> ConvertFromPrimaryServerCurrencyAsync(decimal amount, Currency targetCurrencyCode);

        /// <summary>
        /// Converts currency
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="sourceCurrencyCode">Source currency code</param>
        /// <param name="targetCurrencyCode">Target currency code</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the converted value
        /// </returns>
        Task<decimal> ConvertCurrencyAsync(decimal amount, Currency sourceCurrencyCode, Currency targetCurrencyCode);

        /// <summary>
        /// Converts to primary exchange rate currency 
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="sourceCurrencyCode">Source currency code</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the converted value
        /// </returns>
        Task<decimal> ConvertToPrimaryExchangeRateCurrencyAsync(decimal amount, Currency sourceCurrencyCode);

        /// <summary>
        /// Converts from primary exchange rate currency
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="targetCurrencyCode">Target currency code</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the converted value
        /// </returns>
        Task<decimal> ConvertFromPrimaryExchangeRateCurrencyAsync(decimal amount, Currency targetCurrencyCode);

        #endregion
    }
}