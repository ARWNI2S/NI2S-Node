using ARWNI2S.Infrastructure.Configuration;

namespace ARWNI2S.Node.Core.Entities.Directory
{
    /// <summary>
    /// Measure settings
    /// </summary>
    public partial class MeasureSettings : ISettings
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
    }
}