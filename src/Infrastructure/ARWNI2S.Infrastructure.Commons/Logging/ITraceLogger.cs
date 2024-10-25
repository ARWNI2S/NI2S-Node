using Microsoft.Extensions.Logging;

namespace ARWNI2S.Infrastructure.Logging
{
    /// <summary>
    /// Interface of NI2S runtime for logging services. 
    /// </summary>
    public interface ITraceLogger
    {
        #region APM Methods

        /// <summary>
        /// Used to Track Dependencies.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="commandName"></param>
        /// <param name="startTime"></param>
        /// <param name="duration"></param>
        /// <param name="success"></param>
        void TrackDependency(string name, string commandName, DateTimeOffset startTime, TimeSpan duration, bool success);

        /// <summary>
        /// Used to Track Events.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="properties"></param>
        /// <param name="metrics"></param>
        void TrackEvent(string name, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);

        /// <summary>
        /// Used to Track Metrics.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="properties"></param>
        void TrackMetric(string name, double value, IDictionary<string, string> properties = null);
        /// <summary>
        /// Used to Track Metrics.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="properties"></param>
        void TrackMetric(string name, TimeSpan value, IDictionary<string, string> properties = null);

        /// <summary>
        /// Used to Increment Metrics.
        /// </summary>
        /// <param name="name"></param>
        void IncrementMetric(string name);
        /// <summary>
        /// Used to Increment Metrics.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void IncrementMetric(string name, double value);

        /// <summary>
        /// Used to Decrement Metrics.
        /// </summary>
        /// <param name="name"></param>
        void DecrementMetric(string name);
        /// <summary>
        /// Used to Decrement Metrics.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void DecrementMetric(string name, double value);

        /// <summary>
        /// Used to Track Requests.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="startTime"></param>
        /// <param name="duration"></param>
        /// <param name="responseCode"></param>
        /// <param name="success"></param>
        void TrackRequest(string name, DateTimeOffset startTime, TimeSpan duration, string responseCode, bool success);
        /// <summary>
        /// Used to Track Exceptions.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="properties"></param>
        /// <param name="metrics"></param>
        void TrackException(Exception exception, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);
        /// <summary>
        /// Used to Track Traces.
        /// </summary>
        /// <param name="message"></param>
        void TrackTrace(string message);
        /// <summary>
        /// Used to Track Traces.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="severityLevel"></param>
        void TrackTrace(string message, LogLevel severityLevel);
        /// <summary>
        /// Used to Track Traces.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="severityLevel"></param>
        /// <param name="properties"></param>
        void TrackTrace(string message, LogLevel severityLevel, IDictionary<string, string> properties);
        /// <summary>
        /// Used to Track Traces.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="properties"></param>
        void TrackTrace(string message, IDictionary<string, string> properties);

        #endregion

    }
}
