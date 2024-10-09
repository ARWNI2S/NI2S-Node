using ARWNI2S.Node.Data.Entities.Directory;
using ARWNI2S.Node.Data.Entities.Users;
using ARWNI2S.Node.Data.Services.Plugins;
using ARWNI2S.Node.Data.Services.Users;

namespace ARWNI2S.Node.Data.Services.Directory
{
    /// <summary>
    /// Represents an exchange rate module manager implementation
    /// </summary>
    public partial class ExchangeRateModuleManager : ModuleManager<IExchangeRateProvider>, IExchangeRateModuleManager
    {
        #region Fields

        private readonly CurrencySettings _currencySettings;
        //private readonly TokenSettings _tokenSettings;

        #endregion

        #region Ctor

        public ExchangeRateModuleManager(CurrencySettings currencySettings,
            //TokenSettings tokenSettings,
            IUserService userService,
            IModuleService moduleService) : base(userService, moduleService)
        {
            _currencySettings = currencySettings;
            //_tokenSettings = tokenSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load primary active exchange rate provider
        /// </summary>
        /// <param name="user">Filter by user; pass null to load all modules</param>
        /// <param name="serverId">Filter by server; pass 0 to load all modules</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the exchange rate provider
        /// </returns>
        public virtual async Task<IExchangeRateProvider> LoadCurrencyModuleAsync(User user = null, int serverId = 0)
        {
            return await LoadPrimaryModuleAsync(_currencySettings.ActiveExchangeRateProviderSystemName, user, serverId);
        }

        //public virtual async Task<IExchangeRateProvider> LoadTokenModuleAsync(User user = null, int serverId = 0)
        //{
        //    return await LoadPrimaryModuleAsync(_tokenSettings.ActiveExchangeRateProviderSystemName, user, serverId);
        //}

        /// <summary>
        /// Check whether the passed exchange rate provider is active
        /// </summary>
        /// <param name="exchangeRateProvider">Exchange rate provider to check</param>
        /// <returns>Result</returns>
        public virtual bool IsModuleActive(IExchangeRateProvider exchangeRateProvider)
        {
            return IsModuleActive(exchangeRateProvider, new List<string> { _currencySettings.ActiveExchangeRateProviderSystemName/*, _tokenSettings.ActiveExchangeRateProviderSystemName*/ });
        }


        #endregion
    }
}