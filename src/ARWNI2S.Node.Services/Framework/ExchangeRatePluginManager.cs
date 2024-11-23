﻿using ARWNI2S.Node.Core.Framework;
using ARWNI2S.Node.Data.Entities.Users;
using ARWNI2S.Node.Services.Plugins;
using ARWNI2S.Node.Services.Users;

namespace ARWNI2S.Node.Services.Framework
{
    /// <summary>
    /// Represents an exchange rate plugin manager implementation
    /// </summary>
    public partial class ExchangeRatePluginManager : PluginManager<IExchangeRateProvider>, IExchangeRatePluginManager
    {
        #region Fields

        private readonly FrameworkSettings _frameworkSettings;
        //private readonly TokenSettings _tokenSettings;

        #endregion

        #region Ctor

        public ExchangeRatePluginManager(FrameworkSettings frameworkSettings,
            //TokenSettings tokenSettings,
            IUserService userService,
            IPluginService pluginService) : base(userService, pluginService)
        {
            _frameworkSettings = frameworkSettings;
            //_tokenSettings = tokenSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load primary active exchange rate provider
        /// </summary>
        /// <param name="user">Filter by user; pass null to load all plugins</param>
        /// <param name="serverId">Filter by server; pass 0 to load all plugins</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the exchange rate provider
        /// </returns>
        public virtual async Task<IExchangeRateProvider> LoadCurrencyPluginAsync(User user = null, int serverId = 0)
        {
            return await LoadPrimaryPluginAsync(_frameworkSettings.ActiveExchangeRateProviderSystemName, user, serverId);
        }

        //public virtual async Task<IExchangeRateProvider> LoadTokenPluginAsync(User user = null, int serverId = 0)
        //{
        //    return await LoadPrimaryPluginAsync(_tokenSettings.ActiveExchangeRateProviderSystemName, user, serverId);
        //}

        /// <summary>
        /// Check whether the passed exchange rate provider is active
        /// </summary>
        /// <param name="exchangeRateProvider">Exchange rate provider to check</param>
        /// <returns>Result</returns>
        public virtual bool IsPluginActive(IExchangeRateProvider exchangeRateProvider)
        {
            return IsPluginActive(exchangeRateProvider, [_frameworkSettings.ActiveExchangeRateProviderSystemName/*, _tokenSettings.ActiveExchangeRateProviderSystemName*/]);
        }


        #endregion
    }
}