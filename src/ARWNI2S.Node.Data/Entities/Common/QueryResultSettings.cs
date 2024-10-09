using ARWNI2S.Infrastructure.Configuration;

namespace ARWNI2S.Node.Data.Entities.Common
{
    /// <summary>
    /// Admin area settings
    /// </summary>
    public partial class QueryResultSettings : ISettings
    {
        /// <summary>
        /// Default grid page size
        /// </summary>
        public int DefaultGridPageSize { get; set; }

        /// <summary>
        /// Popup grid page size (for popup pages)
        /// </summary>
        public int PopupGridPageSize { get; set; }

        /// <summary>
        /// A comma-separated list of available grid page sizes
        /// </summary>
        public string GridPageSizes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use IsoDateFormat in JSON results (used for avoiding issue with dates in grids)
        /// </summary>
        public bool UseIsoDateFormatInJsonResult { get; set; }
    }
}