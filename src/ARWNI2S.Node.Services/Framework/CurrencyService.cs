using ARWNI2S.Node.Core;
using ARWNI2S.Node.Core.Framework;
using ARWNI2S.Node.Data;
using ARWNI2S.Node.Data.Entities.Framework;
using ARWNI2S.Node.Data.Extensions;
using ARWNI2S.Node.Services.Clustering;

namespace ARWNI2S.Node.Services.Framework
{
    /// <summary>
    /// Currency service
    /// </summary>
    public partial class CurrencyService : ICurrencyService
    {
        #region Fields

        private readonly FrameworkSettings _frameworkSettings;
        private readonly IExchangeRatePluginManager _exchangeRatePluginManager;
        private readonly IRepository<Currency> _currencyRepository;
        private readonly INodeMappingService _serverMappingService;

        #endregion

        #region Ctor

        public CurrencyService(FrameworkSettings frameworkSettings,
            IExchangeRatePluginManager exchangeRatePluginManager,
            IRepository<Currency> currencyRepository,
            INodeMappingService serverMappingService)
        {
            _frameworkSettings = frameworkSettings;
            _exchangeRatePluginManager = exchangeRatePluginManager;
            _currencyRepository = currencyRepository;
            _serverMappingService = serverMappingService;
        }

        #endregion

        #region Methods

        #region Currency

        /// <summary>
        /// Deletes currency
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteCurrencyAsync(Currency currency)
        {
            await _currencyRepository.DeleteAsync(currency);
        }

        /// <summary>
        /// Gets a currency
        /// </summary>
        /// <param name="currencyId">Currency identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the currency
        /// </returns>
        public virtual async Task<Currency> GetCurrencyByIdAsync(int currencyId)
        {
            return await _currencyRepository.GetByIdAsync(currencyId, cache => default);
        }

        /// <summary>
        /// Gets a currency by code
        /// </summary>
        /// <param name="currencyCode">Currency code</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the currency
        /// </returns>
        public virtual async Task<Currency> GetCurrencyByCodeAsync(string currencyCode)
        {
            if (string.IsNullOrEmpty(currencyCode))
                return null;

#pragma warning disable CA1862 // Use las sobrecargas del método "StringComparison" para realizar comparaciones de cadenas sin distinción de mayúsculas y minúsculas
            return (await GetAllCurrenciesAsync(true))
                .FirstOrDefault(c => c.CurrencyCode.ToLowerInvariant() == currencyCode.ToLowerInvariant());
#pragma warning restore CA1862 // Use las sobrecargas del método "StringComparison" para realizar comparaciones de cadenas sin distinción de mayúsculas y minúsculas
        }

        /// <summary>
        /// Gets all currencies
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="serverId">Load records allowed only in a specified server; pass 0 to load all records</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the currencies
        /// </returns>
        public virtual async Task<IList<Currency>> GetAllCurrenciesAsync(bool showHidden = false, int serverId = 0)
        {
            var currencies = await _currencyRepository.GetAllAsync(query =>
            {
                if (!showHidden)
                    query = query.Where(c => c.Published);

                query = query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id);

                return query;
            }, cache => cache.PrepareKeyForDefaultCache(FrameworkServicesDefaults.CurrenciesAllCacheKey, showHidden));

            //server mapping
            if (serverId > 0)
                currencies = await currencies
                    .WhereAwait(async c => await _serverMappingService.AuthorizeAsync(c, serverId))
                    .ToListAsync();

            return currencies;
        }

        /// <summary>
        /// Inserts a currency
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertCurrencyAsync(Currency currency)
        {
            await _currencyRepository.InsertAsync(currency);
        }

        /// <summary>
        /// Updates the currency
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateCurrencyAsync(Currency currency)
        {
            await _currencyRepository.UpdateAsync(currency);
        }

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
        public virtual async Task<IList<ExchangeRate>> GetCurrencyLiveRatesAsync(string currencyCode = null)
        {
            var exchangeRateProvider = await _exchangeRatePluginManager.LoadCurrencyPluginAsync()
                ?? throw new NiisException("Active exchange rate provider cannot be loaded");

            currencyCode ??= (await GetCurrencyByIdAsync(_frameworkSettings.PrimaryExchangeRateCurrencyId))?.CurrencyCode
                ?? throw new NiisException("Primary exchange rate currency is not set");

            return await exchangeRateProvider.GetCurrencyLiveRatesAsync(currencyCode);
        }

        /// <summary>
        /// Converts currency
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="exchangeRate">Currency exchange rate</param>
        /// <returns>Converted value</returns>
        public virtual decimal ConvertCurrency(decimal amount, decimal exchangeRate)
        {
            if (amount != decimal.Zero && exchangeRate != decimal.Zero)
                return amount * exchangeRate;

            return decimal.Zero;
        }

        /// <summary>
        /// Converts to primary server currency 
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="sourceCurrencyCode">Source currency code</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the converted value
        /// </returns>
        public virtual async Task<decimal> ConvertToPrimaryServerCurrencyAsync(decimal amount, Currency sourceCurrencyCode)
        {
            ArgumentNullException.ThrowIfNull(sourceCurrencyCode);

            var primaryServerCurrency = await GetCurrencyByIdAsync(_frameworkSettings.PrimaryCurrencyId);
            var result = await ConvertCurrencyAsync(amount, sourceCurrencyCode, primaryServerCurrency);

            return result;
        }

        /// <summary>
        /// Converts from primary server currency
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="targetCurrencyCode">Target currency code</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the converted value
        /// </returns>
        public virtual async Task<decimal> ConvertFromPrimaryServerCurrencyAsync(decimal amount, Currency targetCurrencyCode)
        {
            var primaryServerCurrency = await GetCurrencyByIdAsync(_frameworkSettings.PrimaryCurrencyId);
            var result = await ConvertCurrencyAsync(amount, primaryServerCurrency, targetCurrencyCode);

            return result;
        }

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
        public virtual async Task<decimal> ConvertCurrencyAsync(decimal amount, Currency sourceCurrencyCode, Currency targetCurrencyCode)
        {
            ArgumentNullException.ThrowIfNull(sourceCurrencyCode);

            ArgumentNullException.ThrowIfNull(targetCurrencyCode);

            var result = amount;

            if (result == decimal.Zero || sourceCurrencyCode.Id == targetCurrencyCode.Id)
                return result;

            result = await ConvertToPrimaryExchangeRateCurrencyAsync(result, sourceCurrencyCode);
            result = await ConvertFromPrimaryExchangeRateCurrencyAsync(result, targetCurrencyCode);
            return result;
        }

        /// <summary>
        /// Converts to primary exchange rate currency 
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="sourceCurrencyCode">Source currency code</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the converted value
        /// </returns>
        public virtual async Task<decimal> ConvertToPrimaryExchangeRateCurrencyAsync(decimal amount, Currency sourceCurrencyCode)
        {
            ArgumentNullException.ThrowIfNull(sourceCurrencyCode);

            var primaryExchangeRateCurrency = await GetCurrencyByIdAsync(_frameworkSettings.PrimaryExchangeRateCurrencyId) ?? throw new NiisException("Primary exchange rate currency cannot be loaded");
            var result = amount;
            if (result == decimal.Zero || sourceCurrencyCode.Id == primaryExchangeRateCurrency.Id)
                return result;

            var exchangeRate = sourceCurrencyCode.Rate;
            if (exchangeRate == decimal.Zero)
                throw new NiisException($"Exchange rate not found for currency [{sourceCurrencyCode.Name}]");
            result /= exchangeRate;

            return result;
        }

        /// <summary>
        /// Converts from primary exchange rate currency
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <param name="targetCurrencyCode">Target currency code</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the converted value
        /// </returns>
        public virtual async Task<decimal> ConvertFromPrimaryExchangeRateCurrencyAsync(decimal amount, Currency targetCurrencyCode)
        {
            ArgumentNullException.ThrowIfNull(targetCurrencyCode);

            var primaryExchangeRateCurrency = await GetCurrencyByIdAsync(_frameworkSettings.PrimaryExchangeRateCurrencyId) ?? throw new NiisException("Primary exchange rate currency cannot be loaded");
            var result = amount;
            if (result == decimal.Zero || targetCurrencyCode.Id == primaryExchangeRateCurrency.Id)
                return result;

            var exchangeRate = targetCurrencyCode.Rate;
            if (exchangeRate == decimal.Zero)
                throw new NiisException($"Exchange rate not found for currency [{targetCurrencyCode.Name}]");
            result *= exchangeRate;

            return result;
        }

        #endregion

        #endregion
    }
}