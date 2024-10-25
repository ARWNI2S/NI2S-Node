using Microsoft.Extensions.Logging;

namespace ARWNI2S.Infrastructure.Telemetry
{
    /// <summary>
    /// 
    /// </summary>
    interface ITraceTelemetryConsumer : ITelemetryConsumer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void TrackTrace(string message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="severity"></param>
        void TrackTrace(string message, LogLevel severity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="severity"></param>
        /// <param name="properties"></param>
        void TrackTrace(string message, LogLevel severity, IDictionary<string, string> properties);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="properties"></param>
        void TrackTrace(string message, IDictionary<string, string> properties);
    }
}
