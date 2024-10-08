using ARWNI2S.Infrastructure.Logging;

namespace ARWNI2S.Infrastructure.Telemetry.Consumers
{
    /// <summary>
    /// 
    /// </summary>
    public class FileTelemetryConsumer : ITraceTelemetryConsumer
    {
        private StreamWriter _logOutput;
        private readonly object _lockObj = new object();
        private string _logFileName;

        /// <summary>
        /// 
        /// </summary>
        public FileTelemetryConsumer(string fileName)
        {
            _logFileName = TraceLogger.LoggerDirectory + "\\" + fileName;

            if (!File.Exists(_logFileName))
                File.Create(_logFileName).Close();

            _logOutput = new StreamWriter(File.Open(_logFileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
        }

        /// <summary>
        /// 
        /// </summary>
        public void TrackTrace(string message)
        {
            lock (_lockObj)
            {
                if (_logOutput == null) return;

                _logOutput.WriteLine(message);
            }
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
            TrackTrace(message);
        }

        /// <summary>
        /// 
        /// </summary>
        public void TrackTrace(string message, LogLevel traceLevel, IDictionary<string, string> properties)
        {
            TrackTrace(message, properties);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Flush()
        {
            lock (_lockObj)
            {
                if (_logOutput == null) return;

                _logOutput.Flush();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            if (_logOutput == null) return; // was already closed.

            try
            {
                lock (_lockObj)
                {
                    if (_logOutput == null) // was already closed.
                    {
                        return;
                    }
                    _logOutput.Flush();
                    _logOutput.Close();
                }
            }
            catch (Exception exc)
            {
                var msg = string.Format("Ignoring error closing log file {0} - {1}", _logFileName,
                    TraceLogger.PrintException(exc));
                ConsoleText.WriteLine(msg);
            }
            finally
            {
                _logOutput = null;
                _logFileName = null;
            }
        }
    }
}
