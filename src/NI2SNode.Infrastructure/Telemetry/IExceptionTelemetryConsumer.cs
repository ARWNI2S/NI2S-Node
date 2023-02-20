namespace NI2S.Node.Telemetry
{
    /// <summary>
    /// 
    /// </summary>
    interface IExceptionTelemetryConsumer : ITelemetryConsumer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="properties"></param>
        /// <param name="metrics"></param>
        void TrackException(Exception exception, IDictionary<string, string>? properties = null, IDictionary<string, double>? metrics = null);
    }
}
