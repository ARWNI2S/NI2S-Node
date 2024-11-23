using ARWNI2S.Node.Configuration;

namespace ARWNI2S.Node.Core.Framework
{
    /// <summary>
    /// Measure settings
    /// </summary>
    public partial class FrameworkSettings : ISettings
    {
        /// <summary>
        /// Base dimension identifier
        /// </summary>
        public int BaseDimensionId { get; set; }

        /// <summary>
        /// Base weight identifier
        /// </summary>
        public int BaseWeightId { get; set; }

        /// <summary>
        /// Base temperature identifier
        /// </summary>
        public int BaseTemperatureId { get; set; }

        /// <summary>
        /// Primary currency identifier
        /// </summary>
        public int PrimaryCurrencyId { get; set; }

        /// <summary>
        ///  Primary exchange rate currency identifier
        /// </summary>
        public int PrimaryExchangeRateCurrencyId { get; set; }

        /// <summary>
        /// Active exchange rate provider system name (of a plugin)
        /// </summary>
        public string ActiveExchangeRateProviderSystemName { get; set; }

        public bool AutoUpdateEnabled { get; set; }
    }
}