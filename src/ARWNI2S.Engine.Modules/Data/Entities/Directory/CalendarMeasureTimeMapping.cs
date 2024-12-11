namespace ARWNI2S.Node.Core.Entities.Directory
{
    /// <summary>
    /// Represents a calendar-measure time mapping class
    /// </summary>
    public partial class CalendarMeasureTimeMapping : DataEntity
    {
        /// <summary>
        /// Gets or sets the calendar identifier
        /// </summary>
        public int CalendarId { get; set; }

        /// <summary>
        /// Gets or sets the measure time identifier
        /// </summary>
        public int MeasureTimeId { get; set; }
    }
}
