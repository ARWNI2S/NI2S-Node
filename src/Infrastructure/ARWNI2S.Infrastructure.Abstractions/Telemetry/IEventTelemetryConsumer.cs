namespace ARWNI2S.Infrastructure.Telemetry
{
    /// <summary>
    /// 
    /// </summary>
    interface IEventTelemetryConsumer : ITelemetryConsumer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="properties"></param>
        /// <param name="metrics"></param>
        void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);
    }
}
