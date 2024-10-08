using ARWNI2S.Infrastructure.Logging;
using System.Diagnostics;

namespace ARWNI2S.Infrastructure.Telemetry.Consumers
{
    /// <summary>
    /// 
    /// </summary>
    internal class TraceTelemetryConsumer : ITraceTelemetryConsumer
    {
        /// <summary>
        /// 
        /// </summary>
        public void TrackTrace(string message)
        {
            Trace.TraceInformation(message);
        }

        /// <summary>
        /// 
        /// </summary>
        public void TrackTrace(string message, IDictionary<string, string> properties)
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
                    Trace.TraceError(message);
                    break;
                case LogLevel.Info:
                    Trace.TraceInformation(message);
                    break;
                case LogLevel.Verbose:
                case LogLevel.Verbose2:
                case LogLevel.Verbose3:
                    Trace.WriteLine(message);
                    break;
                case LogLevel.Warning:
                    Trace.TraceWarning(message);
                    break;
                case LogLevel.Off:
                    return;
            }
            Trace.Flush();
        }

        /// <summary>
        /// 
        /// </summary>
        public void TrackTrace(string message, LogLevel traceLevel, IDictionary<string, string> properties)
        {
            TrackTrace(TraceParserUtils.PrintProperties(message, properties), traceLevel);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Flush()
        {
            Trace.Flush();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            Trace.Close();
        }
    }
}
