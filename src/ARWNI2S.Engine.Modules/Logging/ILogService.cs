using ARWNI2S.Collections.Generic;
using ARWNI2S.Core.Data.Logging;
using Microsoft.Extensions.Logging;

namespace ARWNI2S.Core.Logging
{
    /// <summary>
    /// Logger interface
    /// </summary>
    public partial interface ILogService //: ILogger<ILogService>
    {
        /// <summary>
        /// Deletes a log item
        /// </summary>
        /// <param name="log">LogRecord item</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteLogAsync(LogRecord log);

        /// <summary>
        /// Deletes a log items
        /// </summary>
        /// <param name="logs">LogRecord items</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteLogsAsync(IList<LogRecord> logs);

        /// <summary>
        /// Clears a log
        /// </summary>
        /// <param name="olderThan">The date that sets the restriction on deleting records. Leave null to remove all records</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task ClearLogAsync(DateTime? olderThan = null);

        /// <summary>
        /// Gets all log items
        /// </summary>
        /// <param name="fromUtc">LogRecord item creation from; null to load all records</param>
        /// <param name="toUtc">LogRecord item creation to; null to load all records</param>
        /// <param name="message">Message</param>
        /// <param name="logLevel">LogRecord level; null to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the log item items
        /// </returns>
        Task<IPagedList<LogRecord>> GetAllLogsAsync(DateTime? fromUtc = null, DateTime? toUtc = null,
            string message = "", LogLevel? logLevel = null,
            int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets a log item
        /// </summary>
        /// <param name="logId">LogRecord item identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the log item
        /// </returns>
        Task<LogRecord> GetLogByIdAsync(int logId);

        /// <summary>
        /// Get log items by identifiers
        /// </summary>
        /// <param name="logIds">LogRecord item identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the log items
        /// </returns>
        Task<IList<LogRecord>> GetLogByIdsAsync(int[] logIds);

        /// <summary>
        /// Inserts a log item
        /// </summary>
        /// <param name="logLevel">LogRecord level</param>
        /// <param name="shortMessage">The short message</param>
        /// <param name="fullMessage">The full message</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains a log item
        /// </returns>
        Task<LogRecord> InsertLogAsync(LogLevel logLevel, string shortMessage, string fullMessage = "");

        /// <summary>
        /// Inserts a log item
        /// </summary>
        /// <param name="logLevel">LogRecord level</param>
        /// <param name="shortMessage">The short message</param>
        /// <param name="fullMessage">The full message</param>
        /// <returns>
        /// LogRecord item
        /// </returns>
        LogRecord InsertLog(LogLevel logLevel, string shortMessage, string fullMessage = "");

        /// <summary>
        /// Information
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InformationAsync(string message, Exception exception = null);

        /// <summary>
        /// Information
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        void Information(string message, Exception exception = null);

        /// <summary>
        /// Warning
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task WarningAsync(string message, Exception exception = null);

        /// <summary>
        /// Warning
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        void Warning(string message, Exception exception = null);

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task ErrorAsync(string message, Exception exception = null);

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        void Error(string message, Exception exception = null);
    }
}
