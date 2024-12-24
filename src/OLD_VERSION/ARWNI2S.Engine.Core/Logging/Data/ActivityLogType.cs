using ARWNI2S.Data.Entities;

namespace ARWNI2S.Engine.Logging.Data
{
    /// <summary>
    /// Represents an activity log type record
    /// </summary>
    public partial class ActivityLogType : DataEntity
    {
        /// <summary>
        /// Gets or sets the system keyword
        /// </summary>
        public string SystemKeyword { get; set; }

        /// <summary>
        /// Gets or sets the display name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the activity log type is enabled
        /// </summary>
        public bool Enabled { get; set; }
    }
}
