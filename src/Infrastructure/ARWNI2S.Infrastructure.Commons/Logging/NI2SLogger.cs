using ARWNI2S.Infrastructure.Telemetry;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace ARWNI2S.Infrastructure.Logging
{
    /// <summary>
    /// Interface of NI2S runtime for logging services. 
    /// </summary>
    [Serializable]
    public abstract class NI2SLogger : ILogger
    {
        /// <summary> 
        /// Current TraceLevelLevel set for this logger.
        /// </summary>
        public abstract LogLevel TraceLevel { get; }

        /// <summary>
        /// Gets/Sets TelemetryConsumers 
        /// </summary>
        public static ConcurrentBag<ITelemetryConsumer> TelemetryConsumers { get; set; }

        /// <summary>
        /// Whether the current TraceLevelLevel would output <c>Warning</c> messages for this logger.
        /// </summary>
        [DebuggerHidden]
        public bool IsWarning
        {
            get { return TraceLevel >= LogLevel.Warning; }
        }

        /// <summary>
        /// Whether the current TraceLevelLevel would output <c>Info</c> messages for this logger.
        /// </summary>
        [DebuggerHidden]
        public bool IsInfo
        {
            get { return TraceLevel >= LogLevel.Info; }
        }

        /// <summary>
        /// Whether the current TraceLevelLevel would output <c>Verbose</c> messages for this logger.
        /// </summary>
        [DebuggerHidden]
        public bool IsVerbose
        {
            get { return TraceLevel >= LogLevel.Verbose; }
        }

        /// <summary>
        /// Whether the current TraceLevelLevel would output <c>Verbose2</c> messages for this logger.
        /// </summary>
        [DebuggerHidden]
        public bool IsVerbose2
        {
            get { return TraceLevel >= LogLevel.Verbose2; }
        }

        /// <summary>
        /// Whether the current TraceLevelLevel would output <c>Verbose3</c> messages for this logger.
        /// </summary>
        [DebuggerHidden]
        public bool IsVerbose3
        {
            get { return TraceLevel >= LogLevel.Verbose3; }
        }

        /// <summary>
        /// Output the specified message at <c>Verbose</c> log level.
        /// </summary>
        public abstract void Verbose(string format, params object[] args);

        /// <summary>
        /// Output the specified message at <c>Verbose2</c> log level.
        /// </summary>
        public abstract void Verbose2(string format, params object[] args);

        /// <summary>
        /// Output the specified message at <c>Verbose3</c> log level.
        /// </summary>
        public abstract void Verbose3(string format, params object[] args);

        /// <summary>
        /// Output the specified message at <c>Info</c> log level.
        /// </summary>
        public abstract void Info(string format, params object[] args);

        #region Public log methods using int LogCode categorization.

        /// <summary>
        /// Output the specified message and Exception at <c>Error</c> log level with the specified log id value.
        /// </summary>
        public abstract void Error(int logCode, string message, Exception exception = null);
        /// <summary>
        /// Output the specified message at <c>Warning</c> log level with the specified log id value.
        /// </summary>
        public abstract void Warn(int logCode, string format, params object[] args);
        /// <summary>
        /// Output the specified message and Exception at <c>Warning</c> log level with the specified log id value.
        /// </summary>
        public abstract void Warn(int logCode, string message, Exception exception = null);
        /// <summary>
        /// Output the specified message at <c>Info</c> log level with the specified log id value.
        /// </summary>
        public abstract void Info(int logCode, string format, params object[] args);
        /// <summary>
        /// Output the specified message at <c>Verbose</c> log level with the specified log id value.
        /// </summary>
        public abstract void Verbose(int logCode, string format, params object[] args);
        /// <summary>
        /// Output the specified message at <c>Verbose2</c> log level with the specified log id value.
        /// </summary>
        public abstract void Verbose2(int logCode, string format, params object[] args);
        /// <summary>
        /// Output the specified message at <c>Verbose3</c> log level with the specified log id value.
        /// </summary>
        public abstract void Verbose3(int logCode, string format, params object[] args);

        #endregion

        #region APM Methods

        /// <summary>
        /// Used to Track Dependencies.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="commandName"></param>
        /// <param name="startTime"></param>
        /// <param name="duration"></param>
        /// <param name="success"></param>
        public abstract void TrackDependency(string name, string commandName, DateTimeOffset startTime, TimeSpan duration, bool success);

        /// <summary>
        /// Used to Track Events.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="properties"></param>
        /// <param name="metrics"></param>
        public abstract void TrackEvent(string name, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);

        /// <summary>
        /// Used to Track Metrics.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="properties"></param>
        public abstract void TrackMetric(string name, double value, IDictionary<string, string> properties = null);
        /// <summary>
        /// Used to Track Metrics.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="properties"></param>
        public abstract void TrackMetric(string name, TimeSpan value, IDictionary<string, string> properties = null);

        /// <summary>
        /// Used to Increment Metrics.
        /// </summary>
        /// <param name="name"></param>
        public abstract void IncrementMetric(string name);
        /// <summary>
        /// Used to Increment Metrics.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public abstract void IncrementMetric(string name, double value);

        /// <summary>
        /// Used to Decrement Metrics.
        /// </summary>
        /// <param name="name"></param>
        public abstract void DecrementMetric(string name);
        /// <summary>
        /// Used to Decrement Metrics.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public abstract void DecrementMetric(string name, double value);

        /// <summary>
        /// Used to Track Requests.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="startTime"></param>
        /// <param name="duration"></param>
        /// <param name="responseCode"></param>
        /// <param name="success"></param>
        public abstract void TrackRequest(string name, DateTimeOffset startTime, TimeSpan duration, string responseCode, bool success);
        /// <summary>
        /// Used to Track Exceptions.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="properties"></param>
        /// <param name="metrics"></param>
        public abstract void TrackException(Exception exception, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);
        /// <summary>
        /// Used to Track Traces.
        /// </summary>
        /// <param name="message"></param>
        public abstract void TrackTrace(string message);
        /// <summary>
        /// Used to Track Traces.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="severityLevel"></param>
        public abstract void TrackTrace(string message, LogLevel severityLevel);
        /// <summary>
        /// Used to Track Traces.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="severityLevel"></param>
        /// <param name="properties"></param>
        public abstract void TrackTrace(string message, LogLevel severityLevel, IDictionary<string, string> properties);
        /// <summary>
        /// Used to Track Traces.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="properties"></param>
        public abstract void TrackTrace(string message, IDictionary<string, string> properties);

        #endregion

        void ILogger.Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            switch (logLevel)
            {
                case Microsoft.Extensions.Logging.LogLevel.Debug:
                    Verbose(eventId.Id, formatter(state, exception), exception);
                    break;
                case Microsoft.Extensions.Logging.LogLevel.Trace:
                case Microsoft.Extensions.Logging.LogLevel.Information:
                    Info(eventId.Id, formatter(state, exception), exception);
                    break;
                case Microsoft.Extensions.Logging.LogLevel.Warning:
                    Warn(eventId.Id, formatter(state, exception), exception);
                    break;
                case Microsoft.Extensions.Logging.LogLevel.Critical:
                case Microsoft.Extensions.Logging.LogLevel.Error:
                    Error(eventId.Id, formatter(state, exception), exception);
                    break;
                case Microsoft.Extensions.Logging.LogLevel.None:
                default:
                    break;
            }
        }

        bool ILogger.IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            return logLevel switch
            {
                Microsoft.Extensions.Logging.LogLevel.Debug => IsVerbose,
                Microsoft.Extensions.Logging.LogLevel.Information => IsInfo,
                Microsoft.Extensions.Logging.LogLevel.Warning => IsWarning,
                Microsoft.Extensions.Logging.LogLevel.Trace or Microsoft.Extensions.Logging.LogLevel.Critical or Microsoft.Extensions.Logging.LogLevel.Error => true,
                _ => false,
            };
        }

        IDisposable ILogger.BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

    }
}
