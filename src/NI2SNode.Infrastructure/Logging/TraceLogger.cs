using NI2S.Node.Configuration;
using NI2S.Node.Interop;
using NI2S.Node.Resources;
using NI2S.Node.Telemetry;
using NI2S.Node.Telemetry.Consumers;
using NI2S.Node.Tools;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Text;

namespace NI2S.Node.Logging
{
    /// <summary>
    /// The TraceLogger class is a convenient wrapper around the .Net Trace class.
    /// It provides more flexible configuration than the Trace class.
    /// </summary>
    public class TraceLogger : Logger
    {
        /// <summary>
        /// The TraceLogger class distinguishes between five categories of loggers:
        /// </summary>
        public enum LoggerType
        {
            /// <summary>
            /// Logs that are written by the NI2S Host. 
            /// </summary>
            Host,
            /// <summary>
            /// Logs that are written by the NI2S run-time itself.
            /// This category should not be used by application code. 
            /// </summary>
            Runtime,
            /// <summary>
            /// Logs that are written by node entities.
            /// This category should be used by code that runs as NI2S entities in a node. 
            /// </summary>
            Entity,
            /// <summary>
            /// Logs that are written by the scene application.
            /// This category should be used by node client-side application code. 
            /// </summary>
            Application,
            /// <summary>
            /// Logs that are written by the node providers.
            /// This category should be used by node providers. 
            /// </summary>
            Provider
        }

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
        private const int LOGGER_INTERN_CACHE_INITIAL_SIZE = InternerConstants.SIZE_MEDIUM;

        #endregion

        #region Static Fields
        /// <summary>
        /// Trave Levels table
        /// </summary>
        public static readonly string[] TraceLevelTable = { "OFF       ", "ERROR     ", "WARNING   ", "INFO      ", "VERBOSE   ", "VERBOSE-2 ", "VERBOSE-3 " };
        public static readonly string LoggerDirectory = Utils.MapPath("~/Log");

        private static readonly ConcurrentDictionary<Type, Func<Exception, string>> exceptionDecoders = new();
        private static readonly object lockable;
        private static readonly List<Tuple<string, LogLevel>> traceLevelOverrides = new();
        private static readonly TimeSpan loggerInternCacheCleanupInterval = InternerConstants.DefaultCacheCleanupFreq;
        private static readonly TimeSpan defaultBulkMessageInterval = TimeSpan.FromMinutes(1);
        /// <summary>
        /// List of log codes that won't have bulk message compaction policy applied to them
        /// </summary>
        private static readonly int[] excludedBulkLogCodes = {
            0,
            (int)TraceCode.NodeRuntime
        };


        private static LogLevel hostTraceLevel =
#if DEBUG
            LogLevel.Info;
#else
            LogLevel.Error;
#endif
        private static LogLevel runtimeTraceLevel =
#if DEBUG
            LogLevel.Info;
#else
            LogLevel.Error;
#endif
        private static LogLevel appTraceLevel =
#if DEBUG
            LogLevel.Warning;
#else
            LogLevel.Error;
#endif
        private static int defaultModificationCounter;
        private static Interner<string, TraceLogger>? loggerStoreInternCache;

        #endregion

        #region Static Properties
        /// <summary>
        /// Whether the NI2S TraceLogger infrastructure has been previously initialized.
        /// </summary>
        public static bool IsInitialized { get; private set; }

        public static IPEndPoint MyIPEndPoint { get; set; }

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
        public static ConcurrentBag<ILogConsumer> LogConsumers { get; private set; }

        #endregion

        #region Fields

        private readonly LoggerType _loggerType;
        private readonly string _logName;

        private int _defaultCopiedCounter;
        private LogLevel _severity;
        private bool _useCustomTraceLevel = false;

        private Dictionary<int, int> recentLogMessageCounts = new();
        private DateTime lastBulkLogMessageFlush = DateTime.MinValue;

        private readonly TimeSpan flushInterval = Debugger.IsAttached ? TimeSpan.FromMilliseconds(10) : TimeSpan.FromSeconds(1);
        private DateTime lastFlush = DateTime.UtcNow;

        #endregion

        #region Properties

        /// <summary>
        /// The current severity level for this TraceLogger.
        /// Log entries will be written if their severity is (logically) equal to or greater than this level.
        /// If it is not explicitly set, then a default value will be calculated based on the logger's type and name.
        /// Note that changes to the global default settings will be propagated to existing loggers that are using the default severity.
        /// </summary>
        public override LogLevel Level
        {
            get
            {
                if (_useCustomTraceLevel || (_defaultCopiedCounter >= defaultModificationCounter)) return _severity;

                _severity = GetDefaultTraceLevelForLog(_logName, _loggerType);
                _defaultCopiedCounter = defaultModificationCounter;
                return _severity;
            }
        }

        #endregion

        #region Static Class

        static TraceLogger()
        {
            //TODO: GIVE VALUE TO MyIPEndPoint
            defaultModificationCounter = 0;
            lockable = new object();
            LogConsumers = new ConcurrentBag<ILogConsumer>();
            TelemetryConsumers = new ConcurrentBag<ITelemetryConsumer>();
            BulkMessageInterval = defaultBulkMessageInterval;
            BulkMessageLimit = Constants.DEFAULT_LOGGER_BULK_MESSAGE_LIMIT;
        }

        #region Initialization

        /// <summary>
        /// Initialize the NI2S TraceLogger subsystem in this process / app domain with the specified configuration settings.
        /// </summary>
        /// <remarks>
        /// In most cases, this call will be made automatically at the approproate poine by the NI2S runtime 
        /// -- must commonly during node initialization and/or node client runtime initialization.
        /// </remarks>
        /// <seealso cref="EntityNodeEntityClient.Initialize()"/>
        /// <param name="config">Configuration settings to be used for initializing the TraceLogger susbystem state.</param>
        protected static void Initialize(ITraceConfiguration config, bool configChange = false)
        {
            if (config == null) throw new ArgumentNullException(nameof(config), LocalizedStrings.TraceLogger_Initialize_ArgumentNullException);

            lock (lockable)
            {
                if (IsInitialized && !configChange) return; // Already initialized

                loggerStoreInternCache = new Interner<string, TraceLogger>(LOGGER_INTERN_CACHE_INITIAL_SIZE, loggerInternCacheCleanupInterval);

                BulkMessageLimit = config.BulkMessageLimit;
                hostTraceLevel = config.DefaultTraceLevel;
                runtimeTraceLevel = config.DefaultTraceLevel;
                appTraceLevel = config.DefaultTraceLevel;
                SetTraceLevelOverrides(config.TraceLevelOverrides);
                defaultModificationCounter++;

                if (configChange)
                    return; // code below should only apply during creation

                if (!Directory.Exists(LoggerDirectory))
                    Directory.CreateDirectory(LoggerDirectory);

                if (config.TraceToConsole)
                {
                    if (!TelemetryConsumers.OfType<ConsoleTelemetryConsumer>().Any())
                    {
                        TelemetryConsumers.Add(new ConsoleTelemetryConsumer());
                    }
                }
                if (!string.IsNullOrEmpty(config.TraceFileName))
                {
                    try
                    {
                        if (!TelemetryConsumers.OfType<FileTelemetryConsumer>().Any())
                        {
                            TelemetryConsumers.Add(new FileTelemetryConsumer(config.TraceFileName));
                        }
                    }
                    catch (Exception exc)
                    {
                        Trace.Listeners.Add(new DefaultTraceListener());
                        Trace.TraceError("Error opening trace file {0} -- Using DefaultTraceListener instead -- Exception={1}", config.TraceFileName, exc);
                    }
                }

                if (Trace.Listeners.Count > 0)
                {
                    if (!TelemetryConsumers.OfType<TraceTelemetryConsumer>().Any())
                    {
                        TelemetryConsumers.Add(new TraceTelemetryConsumer());
                    }
                }

                IsInitialized = true;
            }
        }

        /// <summary>
        /// Uninitialize the NI2S TraceLogger subsystem in this process / app domain.
        /// </summary>
        protected static void UnInitialize()
        {
            lock (lockable)
            {
                Close();
                LogConsumers = new ConcurrentBag<ILogConsumer>();
                TelemetryConsumers = new ConcurrentBag<ITelemetryConsumer>();

                loggerStoreInternCache?.StopAndClear();

                BulkMessageInterval = defaultBulkMessageInterval;
                BulkMessageLimit = Constants.DEFAULT_LOGGER_BULK_MESSAGE_LIMIT;
                IsInitialized = false;
            }
        }

        #endregion

        #region Utils

        private static LogLevel GetDefaultTraceLevelForLog(string source, LoggerType logType)
        {
            string expandedName = logType + "." + source;

            lock (lockable)
            {
                if (traceLevelOverrides.Count > 0)
                {
                    foreach (var o in traceLevelOverrides)
                    {
                        if (expandedName.StartsWith(o.Item1))
                        {
                            return o.Item2;
                        }
                    }
                }
            }

            return logType == LoggerType.Host ? hostTraceLevel : (logType == LoggerType.Runtime ? runtimeTraceLevel : appTraceLevel);
        }

        private static string FormatMessageText(string format, object?[]? args)
        {
            // avoids exceptions if format string contains braces in calls that were not
            // designed to use format strings
            return (args == null || args.Length == 0) ? format : string.Format(format, args);
        }

        private static string PrintException_Helper(Exception? exception, int level, bool includeStackTrace)
        {
            if (exception == null)
                return string.Empty;

            var sb = new StringBuilder();
            sb.Append(PrintOneException(exception, level, includeStackTrace));
            if (exception is ReflectionTypeLoadException rtlEx)
            {
                Exception?[] loaderExceptions = rtlEx.LoaderExceptions;
                if (loaderExceptions == null || loaderExceptions.Length == 0)
                {
                    sb.Append(LocalizedStrings.TraceLogger_PrintException_Helper_NoExceptionsMesage);
                }
                else
                {
                    foreach (Exception? inner in loaderExceptions)
                    {
                        // call recursively on all loader exceptions. Same level for all.
                        sb.Append(PrintException_Helper(inner, level + 1, includeStackTrace));
                    }
                }
            }
            else if (exception is AggregateException aggEx)
            {
                var innerExceptions = aggEx.InnerExceptions;
                if (innerExceptions == null) return sb.ToString();

                foreach (Exception inner in innerExceptions)
                {
                    // call recursively on all inner exceptions. Same level for all.
                    sb.Append(PrintException_Helper(inner, level + 1, includeStackTrace));
                }
            }
            else if (exception.InnerException != null)
            {
                // call recursively on a single inner exception.
                sb.Append(PrintException_Helper(exception.InnerException, level + 1, includeStackTrace));
            }
            return sb.ToString();
        }

        private static string PrintOneException(Exception exception, int level, bool includeStackTrace)
        {
            if (exception == null) return string.Empty;
            string stack = string.Empty;
            if (includeStackTrace && exception.StackTrace != null)
                stack = string.Format(Environment.NewLine + exception.StackTrace);

            string message = exception.Message;
            var excType = exception.GetType();

            if (exceptionDecoders.TryGetValue(excType, out Func<Exception, string>? decoder))
                message = decoder(exception);

            return string.Format(Environment.NewLine + LocalizedStrings.TraceLogger_PrintOneException_Format,
                level,
                exception.GetType(),
                message,
                stack);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Find the TraceLogger with the specified name
        /// </summary>
        /// <param name="loggerName">Name of the TraceLogger to find</param>
        /// <returns>TraceLogger associated with the specified name</returns>
        public static TraceLogger? FindLogger(string loggerName)
        {
            if (loggerStoreInternCache == null) return null;

            loggerStoreInternCache.TryFind(loggerName, out TraceLogger? logger);
            return logger;
        }

        /// <summary>
        /// Find existing or create new TraceLogger with the specified name
        /// </summary>
        /// <param name="loggerName">Name of the TraceLogger to find</param>
        /// <param name="logType">Type of TraceLogger, if it needs to be created</param>
        /// <returns>TraceLogger associated with the specified name</returns>
        public static TraceLogger GetLogger(string loggerName, LoggerType logType)
        {
            return loggerStoreInternCache != null ?
                loggerStoreInternCache.FindOrCreate(loggerName, () => new TraceLogger(loggerName, logType)) :
                new TraceLogger(loggerName, logType);
        }

        public static TraceLogger? GetLogger(string loggerName) => GetLogger(loggerName, LoggerType.Runtime);

        /// <summary>
        /// Set the default log level of all Runtime Loggers.
        /// </summary>
        /// <param name="severity">The new log level to use</param>
        public static void SetRuntimeLogLevel(LogLevel severity)
        {
            runtimeTraceLevel = severity;
            defaultModificationCounter++;
        }

        /// <summary>
        /// Set the default log level of all Entity and Application Loggers.
        /// </summary>
        /// <param name="severity">The new log level to use</param>
        public static void SetAppLogLevel(LogLevel severity)
        {
            appTraceLevel = severity;
            defaultModificationCounter++;
        }

        /// <summary>
        /// Set new trace level overrides for particular loggers, beyond the default log levels.
        /// Any previous trace levels for particular TraceLogger's will be discarded.
        /// </summary>
        /// <param name="overrides">The new set of log level overrided to use.</param>
        public static void SetTraceLevelOverrides(IList<Tuple<string, LogLevel>> overrides)
        {
            List<TraceLogger> loggers;
            lock (lockable)
            {
                traceLevelOverrides.Clear();
                traceLevelOverrides.AddRange(overrides);
                if (traceLevelOverrides.Count > 0)
                {
                    traceLevelOverrides.Sort(new TraceOverrideComparer());
                }
                defaultModificationCounter++;
                loggers = loggerStoreInternCache != null ? loggerStoreInternCache.AllValues() : new List<TraceLogger>();
            }
            foreach (var logger in loggers)
            {
                logger.CheckForTraceLevelOverride();
            }
        }

        /// <summary>
        /// Add a new trace level override for a particular logger, beyond the default log levels.
        /// Any previous trace levels for other TraceLogger's will not be changed.
        /// </summary>
        /// <param name="prefix">The logger names (with prefix matching) that this new log level should apply to.</param>
        /// <param name="level">The new log level to use for this logger.</param>
        public static void AddTraceLevelOverride(string prefix, LogLevel level)
        {
            List<TraceLogger> loggers;
            lock (lockable)
            {
                traceLevelOverrides.Add(new Tuple<string, LogLevel>(prefix, level));
                if (traceLevelOverrides.Count > 0)
                {
                    traceLevelOverrides.Sort(new TraceOverrideComparer());
                }
                loggers = loggerStoreInternCache != null ? loggerStoreInternCache.AllValues() : new List<TraceLogger>();
            }
            foreach (var logger in loggers)
            {
                logger.CheckForTraceLevelOverride();
            }
        }

        /// <summary>
        /// Remove a new trace level override for a particular logger.
        /// The log level for that logger will revert to the current global default setings.
        /// Any previous trace levels for other TraceLogger's will not be changed.
        /// </summary>
        /// <param name="prefix">The logger names (with prefix matching) that this new log level change should apply to.</param>
        public static void RemoveTraceLevelOverride(string prefix)
        {
            List<TraceLogger> loggers;
            lock (lockable)
            {
                var newOverrides = traceLevelOverrides.Where(tuple => !tuple.Item1.Equals(prefix)).ToList();
                traceLevelOverrides.Clear();
                traceLevelOverrides.AddRange(newOverrides);
                if (traceLevelOverrides.Count > 0)
                {
                    traceLevelOverrides.Sort(new TraceOverrideComparer());
                }
                loggers = loggerStoreInternCache != null ? loggerStoreInternCache.AllValues() : new List<TraceLogger>();
            }
            foreach (var logger in loggers)
            {
                if (!logger.MatchesPrefix(prefix)) continue;
                if (logger.CheckForTraceLevelOverride()) continue;

                logger._useCustomTraceLevel = false;
                logger._defaultCopiedCounter = 0;
                logger._severity = GetDefaultTraceLevelForLog(logger._logName, logger._loggerType);
            }
        }

        /// <summary>
        /// Utility function to convert a <c>DateTime</c> object into printable data format used by the TraceLogger subsystem.
        /// </summary>
        /// <param name="date">The <c>DateTime</c> value to be printed.</param>
        /// <returns>Formatted string representation of the input data, in the printable format used by the TraceLogger subsystem.</returns>
        public static string PrintDate(DateTime date)
        {
            return date.ToString(DATE_FORMAT, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Utility function to convert a <c>string</c> object into data format used by the system.
        /// </summary>
        /// <param name="dateStr"></param>
        /// <returns></returns>
        public static DateTime ParseDate(string dateStr)
        {
            return DateTime.ParseExact(dateStr, DATE_FORMAT, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Utility function to convert a <c>DateTime</c> object into printable time format used by the TraceLogger subsystem.
        /// </summary>
        /// <param name="date">The <c>DateTime</c> value to be printed.</param>
        /// <returns>Formatted string representation of the input data, in the printable format used by the TraceLogger subsystem.</returns>
        public static string PrintTime(DateTime date)
        {
            return date.ToString(TIME_FORMAT, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Utility function to convert an exception into printable format, including expanding and formatting any nested sub-expressions.
        /// </summary>
        /// <param name="exception">The exception to be printed.</param>
        /// <returns>Formatted string representation of the exception, including expanding and formatting any nested sub-expressions.</returns>
        public static string PrintException(Exception? exception)
        {
            return exception == null ? string.Empty : PrintException_Helper(exception, 0, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string PrintExceptionWithoutStackTrace(Exception? exception)
        {
            return exception == null ? string.Empty : PrintException_Helper(exception, 0, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptionType"></param>
        /// <param name="decoder"></param>
        public static void SetExceptionDecoder(Type exceptionType, Func<Exception, string> decoder)
        {
            exceptionDecoders.TryAdd(exceptionType, decoder);
        }

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
                    IntPtr.Zero,
                    IntPtr.Zero,
                    IntPtr.Zero);
            }

            return new FileInfo(dumpFileName);
        }

        #endregion

        #endregion

        #region Instanced Class

        #region Constructor

        /// <summary>
        /// Constructs a TraceLogger with the given name and type.
        /// </summary>
        /// <param name="source">The name of the source of log entries for this TraceLogger.
        /// Typically this is the full name of the class that is using this TraceLogger.</param>
        /// <param name="logType">The category of TraceLogger to create.</param>
        protected TraceLogger(string source, LoggerType logType)
        {
            _defaultCopiedCounter = -1;
            _logName = source;
            _loggerType = logType;
            _useCustomTraceLevel = CheckForTraceLevelOverride();
        }

        #endregion

        #region Utils

        private bool MatchesPrefix(string prefix)
        {
            return _logName.StartsWith(prefix, StringComparison.Ordinal)
                || (_loggerType + "." + _logName).StartsWith(prefix, StringComparison.Ordinal);
        }

        private bool CheckForTraceLevelOverride()
        {
            lock (lockable)
            {
                if (traceLevelOverrides.Count <= 0) return false;

                foreach (var o in traceLevelOverrides)
                {
                    if (!MatchesPrefix(o.Item1)) continue;

                    _severity = o.Item2;
                    _useCustomTraceLevel = true;
                    return true;
                }
            }
            return false;
        }

        private void Log(int errorCode, LogLevel sev, string format, object?[]? args, Exception? exception)
        {
            if (sev > Level)
            {
                return;
            }

            if (errorCode == 0 && _loggerType == LoggerType.Runtime)
            {
                errorCode = (int)TraceCode.NodeRuntime;
            }

            if (CheckBulkMessageLimits(errorCode, sev))
            {
                WriteLogMessage(errorCode, sev, format, args, exception);
            }
        }

        private void WriteLogMessage(int errorCode, LogLevel sev, string format, object?[]? args, Exception? exception)
        {
            string message = FormatMessageText(format, args);

            bool logMessageTruncated = false;
            if (message.Length > MAX_LOG_MESSAGE_SIZE)
            {
                message = string.Format(LocalizedStrings.Logger_LogMessageTruncated_Format, message[..MAX_LOG_MESSAGE_SIZE], MAX_LOG_MESSAGE_SIZE);
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
                        consumer.GetType().FullName!, _logName, sev, message, errorCode, exception, exc);
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

            if ((DateTime.UtcNow - lastFlush) > flushInterval)
            {
                lastFlush = DateTime.UtcNow;
                Flush();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set a new severity level for this TraceLogger.
        /// Log entries will be written if their severity is (logically) equal to or greater than this level.
        /// </summary>
        /// <param name="sev">New severity level to be used for filtering log messages.</param>
        public void SetTraceLevel(LogLevel sev)
        {
            _severity = sev;
            _useCustomTraceLevel = true;
        }

        /// <summary>
        /// Writes a log entry at the Verbose severity level.
        /// Verbose is suitable for debugging information that should usually not be logged in production.
        /// Verbose is lower than Info.
        /// </summary>
        /// <param name="format">A standard format string, suitable for string.Format.</param>
        /// <param name="args">Any arguments to the format string.</param>
        public override void Verbose(string format, params object[] args)
        {
            Log(0, LogLevel.Verbose, format, args, null);
        }

        /// <summary>
        /// Writes a log entry at the Verbose2 severity level.
        /// Verbose2 is lower than Verbose.
        /// </summary>
        /// <param name="format">A standard format string, suitable for string.Format.</param>
        /// <param name="args">Any arguments to the format string.</param>
        public override void Verbose2(string format, params object[] args)
        {
            Log(0, LogLevel.Verbose2, format, args, null);
        }

        /// <summary>
        /// Writes a log entry at the Verbose3 severity level.
        /// Verbose3 is the lowest severity level.
        /// </summary>
        /// <param name="format">A standard format string, suitable for string.Format.</param>
        /// <param name="args">Any arguments to the format string.</param>
        public override void Verbose3(string format, params object[] args)
        {
            Log(0, LogLevel.Verbose3, format, args, null);
        }

        /// <summary>
        /// Writes a log entry at the Info severity level.
        /// Info is suitable for information that does not indicate an error but that should usually be logged in production.
        /// Info is lower than Warning.
        /// </summary>
        /// <param name="format">A standard format string, suitable for string.Format.</param>
        /// <param name="args">Any arguments to the format string.</param>
        public override void Info(string format, params object[] args)
        {
            Log(0, LogLevel.Info, format, args, null);
        }

        #region Public log methods using int LogCode categorization.

        /// <summary>
        /// Writes a log entry at the Verbose severity level, with the specified log id code.
        /// Verbose is suitable for debugging information that should usually not be logged in production.
        /// Verbose is lower than Info.
        /// </summary>
        /// <param name="logCode">The log code associated with this message.</param>
        /// <param name="format">A standard format string, suitable for string.Format.</param>
        /// <param name="args">Any arguments to the format string.</param>
        public override void Verbose(int logCode, string format, params object[] args)
        {
            Log(logCode, LogLevel.Verbose, format, args, null);
        }
        /// <summary>
        /// Writes a log entry at the Verbose2 severity level, with the specified log id code.
        /// Verbose2 is lower than Verbose.
        /// </summary>
        /// <param name="logCode">The log code associated with this message.</param>
        /// <param name="format">A standard format string, suitable for string.Format.</param>
        /// <param name="args">Any arguments to the format string.</param>
        public override void Verbose2(int logCode, string format, params object[] args)
        {
            Log(logCode, LogLevel.Verbose2, format, args, null);
        }
        /// <summary>
        /// Writes a log entry at the Verbose3 severity level, with the specified log id code.
        /// Verbose3 is the lowest severity level.
        /// </summary>
        /// <param name="logCode">The log code associated with this message.</param>
        /// <param name="format">A standard format string, suitable for string.Format.</param>
        /// <param name="args">Any arguments to the format string.</param>
        public override void Verbose3(int logCode, string format, params object[] args)
        {
            Log(logCode, LogLevel.Verbose3, format, args, null);
        }
        /// <summary>
        /// Writes a log entry at the Info severity level, with the specified log id code.
        /// Info is suitable for information that does not indicate an error but that should usually be logged in production.
        /// Info is lower than Warning.
        /// </summary>
        /// <param name="logCode">The log code associated with this message.</param>
        /// <param name="format">A standard format string, suitable for string.Format.</param>
        /// <param name="args">Any arguments to the format string.</param>
        public override void Info(int logCode, string format, params object[] args)
        {
            Log(logCode, LogLevel.Info, format, args, null);
        }
        /// <summary>
        /// Writes a log entry at the Warning severity level, with the specified log id code.
        /// Warning is suitable for problem conditions that the system or application can handle by itself,
        /// but that the administrator should be aware of.
        /// Typically these are situations that are expected but that may eventually require an administrative
        /// response if they recur.
        /// Warning is lower than Error.
        /// </summary>
        /// <param name="logCode">The log code associated with this message.</param>
        /// <param name="format">A standard format string, suitable for string.Format.</param>
        /// <param name="args">Any arguments to the format string.</param>
        public override void Warn(int logCode, string format, params object[] args)
        {
            Log(logCode, LogLevel.Warning, format, args, null);
        }
        /// <summary>
        /// Writes a log entry at the Warning severity level, with the specified log id code.
        /// Warning is suitable for problem conditions that the system or application can handle by itself,
        /// but that the administrator should be aware of.
        /// Typically these are situations that are expected but that may eventually require an administrative
        /// response if they recur.
        /// Warning is lower than Error.
        /// </summary>
        /// <param name="logCode">The log code associated with this message.</param>
        /// <param name="message">The warning message to log.</param>
        /// <param name="exception">An exception related to the warning, if any.</param>
        public override void Warn(int logCode, string message, Exception? exception)
        {
            Log(logCode, LogLevel.Warning, message, Array.Empty<object>(), exception);
        }
        /// <summary>
        /// Writes a log entry at the Error severity level, with the specified log id code.
        /// Error is suitable for problem conditions that require immediate administrative response.
        /// </summary>
        /// <param name="logCode">The log code associated with this message.</param>
        /// <param name="message">The error message to log.</param>
        /// <param name="exception">An exception related to the error, if any.</param>
        public override void Error(int logCode, string message, Exception? exception = null)
        {
            Log(logCode, LogLevel.Error, message, Array.Empty<object>(), exception);
        }

        #endregion

        #region Public log methods using ErrorCode categorization.

        public void Verbose(TraceCode errorCode, string format, params object[] args)
        {
            Log((int)errorCode, LogLevel.Verbose, format, args, null);
        }
        public void Verbose2(TraceCode errorCode, string format, params object[] args)
        {
            Log((int)errorCode, LogLevel.Verbose2, format, args, null);
        }
        public void Verbose3(TraceCode errorCode, string format, params object[] args)
        {
            Log((int)errorCode, LogLevel.Verbose3, format, args, null);
        }
        public void Info(TraceCode errorCode, string format, params object[] args)
        {
            Log((int)errorCode, LogLevel.Info, format, args, null);
        }
        public void Warn(TraceCode errorCode, string format, params object[] args)
        {
            Log((int)errorCode, LogLevel.Warning, format, args, null);
        }
        public void Warn(TraceCode errorCode, string message, Exception exception)
        {
            Log((int)errorCode, LogLevel.Warning, message, Array.Empty<object>(), exception);
        }
        public void Error(TraceCode errorCode, string message, Exception? exception = null)
        {
            Log((int)errorCode, LogLevel.Error, message, Array.Empty<object>(), exception);
        }

        #endregion

        // an public method to be used only by the runtime to ensure certain long report messages are logged fully, without truncating and bulking.
        public void LogWithoutBulkingAndTruncating(LogLevel severityLevel, TraceCode errorCode, string format, params object[] args)
        {
            if (severityLevel > Level)
            {
                return;
            }

            string message = FormatMessageText(format, args);
            // skip bulking
            // break into chunks of smaller sizes 
            if (message.Length > MAX_LOG_MESSAGE_SIZE)
            {
                int startIndex = 0;
                int maxChunkSize = MAX_LOG_MESSAGE_SIZE - 100; // 100 bytes to allow slack and prefix.
                int partNum = 1;
                while (startIndex < message.Length)
                {
                    int chunkSize = (startIndex + maxChunkSize) < message.Length ? maxChunkSize : (message.Length - startIndex);
                    var messageToLog = string.Format(LocalizedStrings.TraceLogger_LogWithoutBulkingAndTruncating_Format, partNum, message.Substring(startIndex, chunkSize));
                    WriteLogMessage((int)errorCode, _severity, messageToLog, null, null);
                    startIndex += chunkSize;
                    partNum++;
                }
            }
            else
            {
                WriteLogMessage((int)errorCode, severityLevel, message, null, null);
            }
        }

        public bool CheckBulkMessageLimits(int logCode, LogLevel sev)
        {
            var now = DateTime.UtcNow;
            int count;
            TimeSpan sinceInterval;
            Dictionary<int, int>? copyMessageCounts = null;

            bool isExcluded = excludedBulkLogCodes.Contains(logCode)
                              || (sev == LogLevel.Verbose || sev == LogLevel.Verbose2 || sev == LogLevel.Verbose3);

            lock (this)
            {
                sinceInterval = now - lastBulkLogMessageFlush;
                if (sinceInterval >= BulkMessageInterval)
                {
                    // Take local copy of buffered log message counts, now that this bulk message compaction period has finished
                    copyMessageCounts = recentLogMessageCounts;
                    recentLogMessageCounts = new Dictionary<int, int>();
                    lastBulkLogMessageFlush = now;
                }

                // Increment recent message counts, if appropriate
                if (isExcluded)
                {
                    count = 1;
                    // and don't track counts
                }
                else if (recentLogMessageCounts.TryGetValue(logCode, out int value))
                {
                    count = ++value;
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
                        args[2] = (num == 1) ? "" : "s";

                        WriteLogMessage(ec + BulkMessageSummaryOffset, LogLevel.Info, LocalizedStrings.TraceLogger_CheckBulkMessageLimits_Format, args, null);
                    }
                }
            }

            // Should the current log message be output?
            return isExcluded || (count <= BulkMessageLimit);
        }

        public void Assert(TraceCode errorCode, bool condition, string? message = null)
        {
            if (condition) return;

            message ??= LocalizedStrings.TraceLogger_Assert_Message;

            Fail(errorCode, LocalizedStrings.TraceLogger_Assert_FailMessage + message);
        }

        public void Fail(TraceCode errorCode, string? message)
        {
            message ??= LocalizedStrings.TraceLogger_Fail_Message;

            if (errorCode == 0)
            {
                errorCode = TraceCode.NodeRuntime;
            }

            Error(errorCode, LocalizedStrings.TraceLogger_Fail_ErrorMessage + message + Environment.NewLine + Environment.StackTrace);

            // Create mini-dump of this failure, for later diagnosis
            var dumpFile = CreateMiniDump();
            Error(TraceCode.Logger_MiniDumpCreated, ErrorStrings.Logger_MiniDumpCreated + dumpFile.FullName);

            Flush(); // Flush logs to disk

            // Kill process
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
            else
            {
                Error(TraceCode.Logger_ProcessCrashing, ErrorStrings.Logger_ProcessCrashing);
                Close();

                Environment.FailFast(LocalizedStrings.TraceLogger_Fail_UnrecoverableMessage + message);
            }
        }

        #region APM Methods

        public override void TrackDependency(string name, string commandName, DateTimeOffset startTime, TimeSpan duration, bool success)
        {
            foreach (var tc in TelemetryConsumers.OfType<IDependencyTelemetryConsumer>())
            {
                tc.TrackDependency(name, commandName, startTime, duration, success);
            }
        }

        public override void TrackEvent(string name, IDictionary<string, string>? properties = null, IDictionary<string, double>? metrics = null)
        {
            foreach (var tc in TelemetryConsumers.OfType<IEventTelemetryConsumer>())
            {
                tc.TrackEvent(name, properties, metrics);
            }
        }

        public override void TrackMetric(string name, double value, IDictionary<string, string>? properties = null)
        {
            foreach (var tc in TelemetryConsumers.OfType<IMetricTelemetryConsumer>())
            {
                tc.TrackMetric(name, value, properties);
            }
        }

        public override void TrackMetric(string name, TimeSpan value, IDictionary<string, string>? properties = null)
        {
            foreach (var tc in TelemetryConsumers.OfType<IMetricTelemetryConsumer>())
            {
                tc.TrackMetric(name, value, properties);
            }
        }

        public override void IncrementMetric(string name)
        {
            foreach (var tc in TelemetryConsumers.OfType<IMetricTelemetryConsumer>())
            {
                tc.IncrementMetric(name);
            }
        }

        public override void IncrementMetric(string name, double value)
        {
            foreach (var tc in TelemetryConsumers.OfType<IMetricTelemetryConsumer>())
            {
                tc.IncrementMetric(name, value);
            }
        }

        public override void DecrementMetric(string name)
        {
            foreach (var tc in TelemetryConsumers.OfType<IMetricTelemetryConsumer>())
            {
                tc.DecrementMetric(name);
            }
        }

        public override void DecrementMetric(string name, double value)
        {
            foreach (var tc in TelemetryConsumers.OfType<IMetricTelemetryConsumer>())
            {
                tc.DecrementMetric(name, value);
            }
        }

        public override void TrackRequest(string name, DateTimeOffset startTime, TimeSpan duration, string responseCode, bool success)
        {
            foreach (var tc in TelemetryConsumers.OfType<IRequestTelemetryConsumer>())
            {
                tc.TrackRequest(name, startTime, duration, responseCode, success);
            }
        }

        public override void TrackException(Exception exception, IDictionary<string, string>? properties = null, IDictionary<string, double>? metrics = null)
        {
            foreach (var tc in TelemetryConsumers.OfType<IExceptionTelemetryConsumer>())
            {
                tc.TrackException(exception, properties, metrics);
            }
        }

        public override void TrackTrace(string message)
        {
            foreach (var tc in TelemetryConsumers.OfType<ITraceTelemetryConsumer>())
            {
                tc.TrackTrace(message);
            }
        }

        public override void TrackTrace(string message, LogLevel severity)
        {
            foreach (var tc in TelemetryConsumers.OfType<ITraceTelemetryConsumer>())
            {
                tc.TrackTrace(message, severity);
            }
        }

        public override void TrackTrace(string message, LogLevel severity, IDictionary<string, string> properties)
        {
            foreach (var tc in TelemetryConsumers.OfType<ITraceTelemetryConsumer>())
            {
                tc.TrackTrace(message, severity, properties);
            }
        }

        public override void TrackTrace(string message, IDictionary<string, string> properties)
        {
            foreach (var tc in TelemetryConsumers.OfType<ITraceTelemetryConsumer>())
            {
                tc.TrackTrace(message, properties);
            }
        }

        #endregion

        #endregion

        #endregion

        /// <summary>
        /// This custom comparer lets us sort the TraceLevelOverrides list so that the longest prefix comes first
        /// </summary>
        private class TraceOverrideComparer : Comparer<Tuple<string, LogLevel>>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public override int Compare(Tuple<string, LogLevel>? x, Tuple<string, LogLevel>? y)
            {
                if (x != null)
                    if (y != null)
                        return y.Item1.Length.CompareTo(x.Item1.Length);
                    else
                        return -1;
                else if (y != null)
                    return 1;
                else
                    return 0;
            }
        }
    }
}
