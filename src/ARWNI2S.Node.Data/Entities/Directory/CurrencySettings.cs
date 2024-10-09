using ARWNI2S.Infrastructure.Configuration;

namespace ARWNI2S.Node.Data.Entities.Directory
{
    /// <summary>
    /// Currency settings
    /// </summary>
    public partial class CurrencySettings : ISettings
    {
        /// <summary>
        /// A value indicating whether to display currency labels
        /// </summary>
        public bool DisplayCurrencyLabel { get; set; }

        /// <summary>
        /// Primary node currency identifier
        /// </summary>
        public int PrimaryServerCurrencyId { get; set; }

        /// <summary>
        /// A value indicating whether to allow uset to select display currency
        /// </summary>
        public bool AllowUsersToSelectCurrencyDisplay { get; set; }

        /// <summary>
        ///  Primary exchange rate currency identifier
        /// </summary>
        public int PrimaryExchangeRateCurrencyId { get; set; }

        /// <summary>
        /// Active exchange rate provider system name (of a module)
        /// </summary>
        public string ActiveExchangeRateProviderSystemName { get; set; }

        /// <summary>
        /// A value indicating whether to enable automatic currency rate updates
        /// </summary>
        public bool AutoUpdateEnabled { get; set; }
    }
}