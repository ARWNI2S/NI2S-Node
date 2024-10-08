﻿using ARWNI2S.Infrastructure.Logging;

namespace ARWNI2S.Infrastructure.Telemetry.Consumers
{
    /// <summary>
    /// 
    /// </summary>
    public class ConsoleTelemetryConsumer : ITraceTelemetryConsumer, IExceptionTelemetryConsumer
    {
        /// <summary>
        /// 
        /// </summary>
        public void TrackException(Exception exception, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            ConsoleText.WriteError(TraceParserUtils.PrintProperties(exception.Message, properties), exception);
        }

        /// <summary>
        /// 
        /// </summary>
        public void TrackTrace(string message)
        {
            ConsoleText.WriteLine(message);
        }

        /// <summary>
        /// 
        /// </summary>
        public void TrackTrace(string message, IDictionary<string, string> properties = null)
        {
            TrackTrace(TraceParserUtils.PrintProperties(message, properties));
        }

        /// <summary>
        /// 
        /// </summary>
        public void TrackTrace(string message, LogLevel traceLevel)
        {
            switch (traceLevel)
            {
                case LogLevel.Error:
                    ConsoleText.WriteError(message);
                    break;
                case LogLevel.Info:
                    ConsoleText.WriteStatus(message);
                    break;
                case LogLevel.Verbose:
                case LogLevel.Verbose2:
                case LogLevel.Verbose3:
                    ConsoleText.WriteUsage(message);
                    break;
                case LogLevel.Warning:
                    ConsoleText.WriteWarning(message);
                    break;
                case LogLevel.Off:
                    return;
                default:
                    TrackTrace(message);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void TrackTrace(string message, LogLevel traceLevel, IDictionary<string, string> properties = null)
        {
            TrackTrace(TraceParserUtils.PrintProperties(message, properties));
        }

        /// <summary>
        /// 
        /// </summary>
        public void Flush() { }
        /// <summary>
        /// 
        /// </summary>
        public void Close() { }
    }
}
