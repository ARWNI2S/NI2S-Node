namespace ARWNI2S.Node.Data.Entities.Directory
{
    /// <summary>
    /// Represents an exchange rate
    /// </summary>
    public partial class ExchangeRate
    {
        /// <summary>
        /// Creates a new instance of the ExchangeRate class
        /// </summary>
        public ExchangeRate()
        {
            IsoCode = string.Empty;
            Rate = 1.0m;
        }

        /// <summary>
        /// The three letter ISO code for the Exchange Rate, e.g. USD, BTC
        /// </summary>
        public string IsoCode { get; set; }

        /// <summary>
        /// The conversion rate of this currency from the base currency
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// When was this exchange rate updated from the data source (the data XML feed)
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// Format the rate into a string with the currency code, e.g. "USD 0.72543"
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{IsoCode} {Rate}";
        }
    }
}
