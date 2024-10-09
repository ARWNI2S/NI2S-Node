using ARWNI2S.Infrastructure.Configuration;

namespace ARWNI2S.Node.Core.Services.Helpers
{
    /// <summary>
    /// DateTime settings
    /// </summary>
    public partial class DateTimeSettings : ISettings
    {
        /// <summary>
        /// Gets or sets a default server time zone identifier
        /// </summary>
        public string DefaultServerTimeZoneId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether users are allowed to select theirs time zone
        /// </summary>
        public bool AllowUsersToSetTimeZone { get; set; }
    }
}