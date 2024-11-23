using ARWNI2S.Node.Core.Framework;
using ARWNI2S.Node.Core.Scheduling;

namespace ARWNI2S.Node.Services.Framework
{
    /// <summary>
    /// Represents a task for updating exchange rates
    /// </summary>
    public partial class UpdateExchangeRateTask : IClusterJob
    {
        #region Fields

        private readonly FrameworkSettings _frameworkSettings;
        private readonly ICurrencyService _currencyService;

        #endregion

        #region Ctor

        public UpdateExchangeRateTask(FrameworkSettings frameworkSettings,
            ICurrencyService currencyService)
        {
            _frameworkSettings = frameworkSettings;
            _currencyService = currencyService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes a schedule task
        /// </summary>
        public async Task ExecuteAsync()
        {
            if (_frameworkSettings.AutoUpdateEnabled)
            {
                var exchangeRates = await _currencyService.GetCurrencyLiveRatesAsync();
                foreach (var exchangeRate in exchangeRates)
                {
                    var currency = await _currencyService.GetCurrencyByCodeAsync(exchangeRate.IsoCode);
                    if (currency == null)
                        continue;

                    currency.Rate = exchangeRate.Rate;
                    currency.UpdatedOnUtc = DateTime.UtcNow;
                    await _currencyService.UpdateCurrencyAsync(currency);
                }
            }
        }

        #endregion
    }
}