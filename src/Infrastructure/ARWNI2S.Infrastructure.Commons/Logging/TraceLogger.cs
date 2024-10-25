using ARWNI2S.Infrastructure.Interop;
using ARWNI2S.Infrastructure.Resources;
using ARWNI2S.Infrastructure.Telemetry;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace ARWNI2S.Infrastructure.Logging
{
    internal class TraceLogger : ILogger
    {
        #region Constants

        /// <summary>
        /// Maximum length of log messages. 
        /// Log messages about this size will be truncated.
        /// </summary>
        public const int MAX_LOG_MESSAGE_SIZE = 20000;
        public const int BulkMessageSummaryOffset = 500000;
        // http://www.csharp-examples.net/string-format-DateTime/
        // http://msdn.microsoft.com/en-us/library/system.globalization.DateTimeformatinfo.aspx
        private const string TIME_FORMAT = "HH:mm:ss.fff 'GMT'"; // Example: 09:50:43.341 GMT
        private const string DATE_FORMAT = "yyyy-MM-dd " + TIME_FORMAT; // Example: 2010-09-02 09:50:43.341 GMT - Variant of UniversalSorta­bleDateTimePat­tern

        #endregion

        /// <summary>
        /// Count limit for bulk message output.
        /// If the same log code is written more than <c>BulkMessageLimit</c> times in the <c>BulkMessageInterval</c> time period, 
        /// then only the first <c>BulkMessageLimit</c> individual messages will be written, plus a count of how bulk messages suppressed.
        /// </summary>
        public static int BulkMessageLimit { get; set; }

        /// <summary>
        /// Time limit for bulk message output.
        /// If the same log code is written more than <c>BulkMessageLimit</c> times in the <c>BulkMessageInterval</c> time period, 
        /// then only the first <c>BulkMessageLimit</c> individual messages will be written, plus a count of how bulk messages suppressed.
        /// </summary>
        public static TimeSpan BulkMessageInterval { get; set; }

        /// <summary>
        /// The set of <see cref="ILogConsumer"/> references to write log events to. 
        /// </summary>
        public static ConcurrentBag<ILogConsumer> LogConsumers { get; private set; } = [];
        /// <summary>
        /// Gets/Sets TelemetryConsumers 
        /// </summary>
        public static ConcurrentBag<ITelemetryConsumer> TelemetryConsumers { get; set; } = [];

        /// <summary>
        /// List of log codes that won't have bulk message compaction policy applied to them
        /// </summary>
        private static readonly int[] excludedBulkLogCodes = {
            0,
            (int)TraceCode.NodeRuntime
        };


        #region Fields

        private readonly string _logName;

        private Dictionary<int, int> recentLogMessageCounts = [];
        private DateTime lastBulkLogMessageFlush = DateTime.MinValue;

        private readonly TimeSpan flushInterval = Debugger.IsAttached ? TimeSpan.FromMilliseconds(10) : TimeSpan.FromSeconds(1);
        private DateTime lastFlush = DateTime.UtcNow;

        #endregion

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state)
        {
            return NullScope.Instance;
        }

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        /// <inheritdoc />
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Log(eventId.Id, logLevel, formatter(state, exception), [], exception);
        }

        #region Utils

        private static string FormatMessageText(string format, object[] args)
        {
            // avoids exceptions if format string contains braces in calls that were not
            // designed to use format strings
            return args == null || args.Length == 0 ? format : string.Format(format, args);
        }

        private void Log(int errorCode, LogLevel sev, string format, object[] args, Exception exception)
        {
            if (!IsEnabled(sev))
                return;

            if (errorCode == 0)
            {
                errorCode = (int)TraceCode.NodeRuntime;
            }

            if (CheckBulkMessageLimits(errorCode, sev))
            {
                WriteLogMessage(errorCode, sev, format, args, exception);
            }
        }

        private void WriteLogMessage(int errorCode, LogLevel sev, string format, object[] args, Exception exception)
        {
            string message = FormatMessageText(format, args);

            bool logMessageTruncated = false;
            if (message.Length > MAX_LOG_MESSAGE_SIZE)
            {
                message = string.Format(LocalizedStrings.Logger_LogMessageTruncated_Format, message.Substring(0, MAX_LOG_MESSAGE_SIZE), MAX_LOG_MESSAGE_SIZE);
                logMessageTruncated = true;
            }

            foreach (ILogConsumer consumer in LogConsumers)
            {
                try
                {
                    consumer.Log(sev, _loggerType, _logName, message, MyIPEndPoint, exception, errorCode);

                    if (logMessageTruncated)
                    {
                        consumer.Log(LogLevel.Warning, _loggerType, _logName,
                            LocalizedStrings.TraceLogger_WriteLogMessage_MessageTruncatedText + MAX_LOG_MESSAGE_SIZE,
                            MyIPEndPoint, exception,
                            (int)TraceCode.Logger_LogMessageTruncated);
                    }
                }
                catch (Exception exc)
                {
                    ConsoleText.WriteLine(LocalizedStrings.TraceLogger_WriteLogMessage_ExceptionFormat,
                        consumer.GetType().FullName, _logName, sev, message, errorCode, exception, exc);
                }
            }

            var formatedTraceMessage = TraceParserUtils.FormatLogMessage(sev, _loggerType, _logName, message, MyIPEndPoint, exception, errorCode);

            if (exception != null)
                TrackException(exception);

            TrackTrace(formatedTraceMessage, sev);

            if (logMessageTruncated)
            {
                formatedTraceMessage = TraceParserUtils.FormatLogMessage(LogLevel.Warning, _loggerType, _logName,
                    LocalizedStrings.TraceLogger_WriteLogMessage_MessageTruncatedText + MAX_LOG_MESSAGE_SIZE,
                    MyIPEndPoint, exception,
                    (int)TraceCode.Logger_LogMessageTruncated);

                TrackTrace(formatedTraceMessage);
            }

            if (DateTime.UtcNow - lastFlush > flushInterval)
            {
                lastFlush = DateTime.UtcNow;
                Flush();
            }
        }

        private bool CheckBulkMessageLimits(int logCode, LogLevel sev)
        {
            var now = DateTime.UtcNow;
            int count;
            TimeSpan sinceInterval;
            Dictionary<int, int> copyMessageCounts = null;

            bool isExcluded = excludedBulkLogCodes.Contains(logCode)
                              || sev == LogLevel.Debug;

            lock (this)
            {
                sinceInterval = now - lastBulkLogMessageFlush;
                if (sinceInterval >= BulkMessageInterval)
                {
                    // Take local copy of buffered log message counts, now that this bulk message compaction period has finished
                    copyMessageCounts = recentLogMessageCounts;
                    recentLogMessageCounts = [];
                    lastBulkLogMessageFlush = now;
                }

                // Increment recent message counts, if appropriate
                if (isExcluded)
                {
                    count = 1;
                    // and don't track counts
                }
                else if (recentLogMessageCounts.ContainsKey(logCode))
                {
                    count = ++recentLogMessageCounts[logCode];
                }
                else
                {
                    recentLogMessageCounts.Add(logCode, 1);
                    count = 1;
                }
            }

            // Output any pending bulk compaction messages
            if (copyMessageCounts != null && copyMessageCounts.Count > 0)
            {
                object[] args = new object[4];
                args[3] = sinceInterval;

                // Output summary counts for any pending bulk message occurrances
                foreach (int ec in copyMessageCounts.Keys)
                {
                    int num = copyMessageCounts[ec] - BulkMessageLimit;

                    // Only output log codes which exceeded limit threshold
                    if (num > 0)
                    {
                        args[0] = ec;
                        args[1] = num;
                        args[2] = num == 1 ? "" : "s";

                        WriteLogMessage(ec + BulkMessageSummaryOffset, LogLevel.Information, LocalizedStrings.TraceLogger_CheckBulkMessageLimits_Format, args, null);
                    }
                }
            }

            // Should the current log message be output?
            return isExcluded || count <= BulkMessageLimit;
        }

        #endregion

        #region APM Methods

        public virtual void TrackDependency(string name, string commandName, DateTimeOffset startTime, TimeSpan duration, bool success)
        {
            foreach (var tc in TelemetryConsumers.OfType<IDependencyTelemetryConsumer>())
            {
                tc.TrackDependency(name, commandName, startTime, duration, success);
            }
        }

        public virtual void TrackEvent(string name, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            foreach (var tc in TelemetryConsumers.OfType<IEventTelemetryConsumer>())
            {
                tc.TrackEvent(name, properties, metrics);
            }
        }

        public virtual void TrackMetric(string name, double value, IDictionary<string, string> properties = null)
        {
            foreach (var tc in TelemetryConsumers.OfType<IMetricTelemetryConsumer>())
            {
                tc.TrackMetric(name, value, properties);
            }
        }

        public virtual void TrackMetric(string name, TimeSpan value, IDictionary<string, string> properties = null)
        {
            foreach (var tc in TelemetryConsumers.OfType<IMetricTelemetryConsumer>())
            {
                tc.TrackMetric(name, value, properties);
            }
        }

        public virtual void IncrementMetric(string name)
        {
            foreach (var tc in TelemetryConsumers.OfType<IMetricTelemetryConsumer>())
            {
                tc.IncrementMetric(name);
            }
        }

        public virtual void IncrementMetric(string name, double value)
        {
            foreach (var tc in TelemetryConsumers.OfType<IMetricTelemetryConsumer>())
            {
                tc.IncrementMetric(name, value);
            }
        }

        public virtual void DecrementMetric(string name)
        {
            foreach (var tc in TelemetryConsumers.OfType<IMetricTelemetryConsumer>())
            {
                tc.DecrementMetric(name);
            }
        }

        public virtual void DecrementMetric(string name, double value)
        {
            foreach (var tc in TelemetryConsumers.OfType<IMetricTelemetryConsumer>())
            {
                tc.DecrementMetric(name, value);
            }
        }

        public virtual void TrackRequest(string name, DateTimeOffset startTime, TimeSpan duration, string responseCode, bool success)
        {
            foreach (var tc in TelemetryConsumers.OfType<IRequestTelemetryConsumer>())
            {
                tc.TrackRequest(name, startTime, duration, responseCode, success);
            }
        }

        public virtual void TrackException(Exception exception, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            foreach (var tc in TelemetryConsumers.OfType<IExceptionTelemetryConsumer>())
            {
                tc.TrackException(exception, properties, metrics);
            }
        }

        public virtual void TrackTrace(string message)
        {
            foreach (var tc in TelemetryConsumers.OfType<ITraceTelemetryConsumer>())
            {
                tc.TrackTrace(message);
            }
        }

        public virtual void TrackTrace(string message, LogLevel severity)
        {
            foreach (var tc in TelemetryConsumers.OfType<ITraceTelemetryConsumer>())
            {
                tc.TrackTrace(message, severity);
            }
        }

        public virtual void TrackTrace(string message, LogLevel severity, IDictionary<string, string> properties)
        {
            foreach (var tc in TelemetryConsumers.OfType<ITraceTelemetryConsumer>())
            {
                tc.TrackTrace(message, severity, properties);
            }
        }

        public virtual void TrackTrace(string message, IDictionary<string, string> properties)
        {
            foreach (var tc in TelemetryConsumers.OfType<ITraceTelemetryConsumer>())
            {
                tc.TrackTrace(message, properties);
            }
        }

        #endregion

        /// <summary>
        /// Attempt to flush any pending trace log writes to disk / backing store
        /// </summary>
        public static void Flush()
        {
            try
            {
                // Flush trace logs to disk
                Trace.Flush();

                foreach (IFlushableLogConsumer consumer in LogConsumers.OfType<IFlushableLogConsumer>())
                {
                    try
                    {
                        consumer.Flush();
                    }
                    catch (Exception) { }
                }

                foreach (var consumer in TelemetryConsumers)
                {
                    try
                    {
                        consumer.Flush();
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception) { }
        }

        public static void Close()
        {
            Flush();
            try
            {
                foreach (ICloseableLogConsumer consumer in LogConsumers.OfType<ICloseableLogConsumer>())
                {
                    try
                    {
                        consumer.Close();
                    }
                    catch (Exception) { }
                }

                foreach (var consumer in TelemetryConsumers)
                {
                    try
                    {
                        consumer.Close();
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Create a mini-dump file for the current state of this process
        /// </summary>
        /// <param name="dumpType">Type of mini-dump to create</param>
        /// <returns><c>FileInfo</c> for the location of the newly created mini-dump file</returns>
        public static FileInfo CreateMiniDump(MiniDumpType dumpType = MiniDumpType.MiniDumpNormal)
        {
            const string dateFormat = "yyyy-MM-dd-HH-mm-ss-fffZ"; // Example: 2010-09-02-09-50-43-341Z

            var thisAssembly = (Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly()) ?? typeof(TraceLogger).GetTypeInfo().Assembly;

            var dumpFileName = string.Format(@"{0}-MiniDump-{1}.dmp",
                thisAssembly.GetName().Name,
                DateTime.UtcNow.ToString(dateFormat, CultureInfo.InvariantCulture));

            using (var stream = File.Create(dumpFileName))
            {
                var process = Process.GetCurrentProcess();

                // It is safe to call DangerousGetHandle() here because the process is already crashing.
                NativeMethods.MiniDumpWriteDump(
                    process.Handle,
                    process.Id,
                    stream.SafeFileHandle.DangerousGetHandle(),
                    dumpType,
                    nint.Zero,
                    nint.Zero,
                    nint.Zero);
            }

            return new FileInfo(dumpFileName);
        }

    }
}
