namespace ARWNI2S.Infrastructure.Telemetry
{
    /// <summary>
    /// 
    /// </summary>
    interface IMetricTelemetryConsumer : ITelemetryConsumer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="properties"></param>
        void TrackMetric(string name, double value, IDictionary<string, string> properties = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="properties"></param>
        void TrackMetric(string name, TimeSpan value, IDictionary<string, string> properties = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        void IncrementMetric(string name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void IncrementMetric(string name, double value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        void DecrementMetric(string name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void DecrementMetric(string name, double value);
    }
}
