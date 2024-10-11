using ARWNI2S.Node.Data.Entities.Directory;
using ARWNI2S.Node.Data.Services.ScheduleTasks;

namespace ARWNI2S.Node.Data.Services.Directory
{
    /// <summary>
    /// Represents a task for updating exchange rates
    /// </summary>
    public partial class UpdateExchangeRateTask : IScheduleTask
    {
        #region Fields

        private readonly CurrencySettings _currencySettings;
        private readonly ICurrencyService _currencyService;

        #endregion

        #region Ctor

        public UpdateExchangeRateTask(CurrencySettings currencySettings,
            ICurrencyService currencyService)
        {
            _currencySettings = currencySettings;
            _currencyService = currencyService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes a schedule task
        /// </summary>
        public async Task ExecuteAsync()
        {
            if (_currencySettings.AutoUpdateEnabled)
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