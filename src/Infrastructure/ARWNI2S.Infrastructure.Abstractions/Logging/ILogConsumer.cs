using Microsoft.Extensions.Logging;
using System.Net;

namespace ARWNI2S.Infrastructure.Logging
{
    /// <summary>
    /// An interface used to consume log entries. 
    /// </summary>
    public interface ILogConsumer
    {
        /// <summary>
        /// The method to call during logging.
        /// This method should be very fast, since it is called synchronously during NI2S logging.
        /// </summary>
        /// <param name="level">The severity of the message being traced.</param>
        /// <param name="caller">The name of the logger tracing the message.</param>
        /// <param name="myIPEndPoint">The <see cref="IPEndPoint"/> of the NI2S node client/server if known. May be null.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log. May be null.</param>
        /// <param name="errorCode">Numeric event code for this log entry. May be zero, meaning 'Unspecified'. 
        /// In general, all log entries at severity=Error or greater should specify an explicit error code value.</param>
        void Log(
            LogLevel level,
            string caller,
            string message,
            IPEndPoint myIPEndPoint,
            Exception exception,
            int errorCode = 0
            );
    }

    /// <summary>
    /// An interface used to consume log entries, when a Flush function is also supported. 
    /// </summary>
    public interface IFlushableLogConsumer : ILogConsumer
    {
        /// <summary>
        /// Flush any pending log writes.
        /// </summary>
        void Flush();
    }

    /// <summary>
    /// An interface used to consume log entries, when a Close function is also supported. 
    /// </summary>
    public interface ICloseableLogConsumer : ILogConsumer
    {
        /// <summary>
        /// Close this log.
        /// </summary>
        void Close();
    }
}