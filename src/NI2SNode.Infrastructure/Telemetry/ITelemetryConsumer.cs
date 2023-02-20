namespace NI2S.Node.Telemetry
{
    /// <summary>
    /// Marker interface for all Telemetry Consumers
    /// </summary>
    public interface ITelemetryConsumer
    {
        /// <summary>
        /// Flush ITelemetryConsumer. 
        /// </summary>
        void Flush();

        /// <summary>
        /// Close ITelemetryConsumer. 
        /// </summary>
        void Close();
    }
}
