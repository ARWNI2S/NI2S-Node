using ARWNI2S.Infrastructure.Collections.Generic;
using ARWNI2S.Infrastructure.Entities;
using ARWNI2S.Infrastructure.Logging;
using ARWNI2S.Node.Core;
using ARWNI2S.Node.Core.Common;
using ARWNI2S.Node.Data;
using ARWNI2S.Node.Data.Entities.Logging;

namespace ARWNI2S.Node.Services.Logging
{
    /// <summary>
    /// Default logger
    /// </summary>
    public partial class DefaultLogger(CommonSettings commonSettings,
        IRepository<Log> logRepository,
        INodeHelper nodeHelper) : ILogService
    {

        #region Utilities

        /// <summary>
        /// Gets a value indicating whether this message should not be logged
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Result</returns>
        protected virtual bool IgnoreLog(string message)
        {
            if (commonSettings.IgnoreLogWordlist.Count == 0)
                return false;

            if (string.IsNullOrWhiteSpace(message))
                return false;

            return commonSettings
                .IgnoreLogWordlist
                .Any(x => message.Contains(x, StringComparison.InvariantCultureIgnoreCase));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether a log level is enabled
        /// </summary>
        /// <param name="level">Log level</param>
        /// <returns>Result</returns>
        public virtual bool IsEnabled(LogLevel level)
        {
            return level switch
            {
                LogLevel.Verbose or LogLevel.Verbose2 or LogLevel.Verbose3 => false,
                _ => true,
            };
        }

        /// <summary>
        /// Deletes a log item
        /// </summary>
        /// <param name="log">Log item</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteLogAsync(Log log)
        {
            ArgumentNullException.ThrowIfNull(log);

            await logRepository.DeleteAsync(log, false);
        }

        /// <summary>
        /// Deletes a log items
        /// </summary>
        /// <param name="logs">Log items</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteLogsAsync(IList<Log> logs)
        {
            await logRepository.DeleteAsync(logs, false);
        }

        /// <summary>
        /// Clears a log
        /// </summary>
        /// <param name="olderThan">The date that sets the restriction on deleting records. Leave null to remove all records</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task ClearLogAsync(DateTime? olderThan = null)
        {
            if (olderThan == null)
                await logRepository.TruncateAsync();
            else
                await logRepository.DeleteAsync(p => p.CreatedOnUtc < olderThan.Value);
        }

        /// <summary>
        /// Gets all log items
        /// </summary>
        /// <param name="fromUtc">Log item creation from; null to load all records</param>
        /// <param name="toUtc">Log item creation to; null to load all records</param>
        /// <param name="message">Message</param>
        /// <param name="logLevel">Log level; null to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the log item items
        /// </returns>
        public virtual async Task<IPagedList<Log>> GetAllLogsAsync(DateTime? fromUtc = null, DateTime? toUtc = null,
            string message = "", LogLevel? logLevel = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var logs = await logRepository.GetAllPagedAsync(query =>
            {
                if (fromUtc.HasValue)
                    query = query.Where(l => fromUtc.Value <= l.CreatedOnUtc);
                if (toUtc.HasValue)
                    query = query.Where(l => toUtc.Value >= l.CreatedOnUtc);
                if (logLevel.HasValue)
                {
                    var logLevelId = (int)logLevel.Value;
                    query = query.Where(l => logLevelId == l.LogLevelId);
                }

                if (!string.IsNullOrEmpty(message))
                    query = query.Where(l => l.ShortMessage.Contains(message) || l.FullMessage.Contains(message));
                query = query.OrderByDescending(l => l.CreatedOnUtc);

                return query;
            }, pageIndex, pageSize);

            return logs;
        }

        /// <summary>
        /// Gets a log item
        /// </summary>
        /// <param name="logId">Log item identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the log item
        /// </returns>
        public virtual async Task<Log> GetLogByIdAsync(int logId)
        {
            return await logRepository.GetByIdAsync(logId);
        }

        /// <summary>
        /// Get log items by identifiers
        /// </summary>
        /// <param name="logIds">Log item identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the log items
        /// </returns>
        public virtual async Task<IList<Log>> GetLogByIdsAsync(int[] logIds)
        {
            return await logRepository.GetByIdsAsync(logIds);
        }

        /// <summary>
        /// Inserts a log item
        /// </summary>
        /// <param name="logLevel">Log level</param>
        /// <param name="shortMessage">The short message</param>
        /// <param name="fullMessage">The full message</param>
        /// <param name="user">The user to associate log record with</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains a log item
        /// </returns>
        public virtual async Task<Log> InsertLogAsync(LogLevel logLevel, string shortMessage, string fullMessage = "", INI2SUser user = null)
        {
            //check ignore word/phrase list?
            if (IgnoreLog(shortMessage) || IgnoreLog(fullMessage))
                return null;

            var log = new Log
            {
                LogLevel = logLevel,
                ShortMessage = shortMessage,
                FullMessage = fullMessage,
                IpAddress = nodeHelper.GetCurrentIpAddress(),
                UserId = user?.Id,
                CreatedOnUtc = DateTime.UtcNow
            };

            await logRepository.InsertAsync(log, false);

            return log;
        }

        /// <summary>
        /// Inserts a log item
        /// </summary>
        /// <param name="logLevel">Log level</param>
        /// <param name="shortMessage">The short message</param>
        /// <param name="fullMessage">The full message</param>
        /// <param name="user">The user to associate log record with</param>
        /// <returns>
        /// Log item
        /// </returns>
        public virtual Log InsertLog(LogLevel logLevel, string shortMessage, string fullMessage = "", INI2SUser user = null)
        {
            //check ignore word/phrase list?
            if (IgnoreLog(shortMessage) || IgnoreLog(fullMessage))
                return null;

            var log = new Log
            {
                LogLevel = logLevel,
                ShortMessage = shortMessage,
                FullMessage = fullMessage,
                IpAddress = nodeHelper.GetCurrentIpAddress(),
                UserId = user?.Id,
                CreatedOnUtc = DateTime.UtcNow
            };

            logRepository.Insert(log, false);

            return log;
        }

        /// <summary>
        /// Information
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        /// <param name="user">User</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InformationAsync(string message, Exception exception = null, INI2SUser user = null)
        {
            //don't log thread abort exception
            if (exception is ThreadAbortException)
                return;

            if (IsEnabled(LogLevel.Info))
                await InsertLogAsync(LogLevel.Info, message, exception?.ToString() ?? string.Empty, user);
        }

        /// <summary>
        /// Information
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        /// <param name="user">User</param>
        public virtual void Information(string message, Exception exception = null, INI2SUser user = null)
        {
            //don't log thread abort exception
            if (exception is ThreadAbortException)
                return;

            if (IsEnabled(LogLevel.Info))
                InsertLog(LogLevel.Info, message, exception?.ToString() ?? string.Empty, user);
        }

        /// <summary>
        /// Warning
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        /// <param name="user">User</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task WarningAsync(string message, Exception exception = null, INI2SUser user = null)
        {
            //don't log thread abort exception
            if (exception is ThreadAbortException)
                return;

            if (IsEnabled(LogLevel.Warning))
                await InsertLogAsync(LogLevel.Warning, message, exception?.ToString() ?? string.Empty, user);
        }

        /// <summary>
        /// Warning
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        /// <param name="user">User</param>
        public virtual void Warning(string message, Exception exception = null, INI2SUser user = null)
        {
            //don't log thread abort exception
            if (exception is ThreadAbortException)
                return;

            if (IsEnabled(LogLevel.Warning))
                InsertLog(LogLevel.Warning, message, exception?.ToString() ?? string.Empty, user);
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        /// <param name="user">User</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task ErrorAsync(string message, Exception exception = null, INI2SUser user = null)
        {
            //don't log thread abort exception
            if (exception is ThreadAbortException)
                return;

            if (IsEnabled(LogLevel.Error))
                await InsertLogAsync(LogLevel.Error, message, exception?.ToString() ?? string.Empty, user);
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        /// <param name="user">User</param>
        public virtual void Error(string message, Exception exception = null, INI2SUser user = null)
        {
            //don't log thread abort exception
            if (exception is ThreadAbortException)
                return;

            if (IsEnabled(LogLevel.Error))
                InsertLog(LogLevel.Error, message, exception?.ToString() ?? string.Empty, user);
        }

        #endregion
    }
}
