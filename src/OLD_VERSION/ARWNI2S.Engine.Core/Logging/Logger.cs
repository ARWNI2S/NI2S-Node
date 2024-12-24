using ARWNI2S.Data;
using ARWNI2S.Engine.Collections.Generic;
using ARWNI2S.Engine.Common.Data;
using ARWNI2S.Engine.Logging.Data;
using Microsoft.Extensions.Logging;

namespace ARWNI2S.Engine.Logging
{
    /// <summary>
    /// Default logger
    /// </summary>
    public partial class Logger : ILogService
    {
        private readonly CommonSettings _commonSettings;
        private readonly IRepository<LogRecord> _logRepository;
        //private readonly INiisHelper _nodeHelper;

        public Logger(CommonSettings commonSettings,
            IRepository<LogRecord> logRepository)//,
                                                 //INiisHelper nodeHelper)
        {
            _commonSettings = commonSettings;
            _logRepository = logRepository;
            //_nodeHelper = nodeHelper;
        }

        #region Utilities

        /// <summary>
        /// Gets a value indicating whether this message should not be logged
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Result</returns>
        protected virtual bool IgnoreLog(string message)
        {
            if (_commonSettings.IgnoreLogWordlist.Count == 0)
                return false;

            if (string.IsNullOrWhiteSpace(message))
                return false;

            return _commonSettings
                .IgnoreLogWordlist
                .Any(x => message.Contains(x, StringComparison.InvariantCultureIgnoreCase));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether a log level is enabled
        /// </summary>
        /// <param name="level">LogRecord level</param>
        /// <returns>Result</returns>
        public virtual bool IsEnabled(LogLevel level)
        {
            return level switch
            {
                LogLevel.Debug => false,
                _ => true,
            };
        }

        /// <summary>
        /// Deletes a log item
        /// </summary>
        /// <param name="log">LogRecord item</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteLogAsync(LogRecord log)
        {
            ArgumentNullException.ThrowIfNull(log);

            await _logRepository.DeleteAsync(log, false);
        }

        /// <summary>
        /// Deletes a log items
        /// </summary>
        /// <param name="logs">LogRecord items</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteLogsAsync(IList<LogRecord> logs)
        {
            await _logRepository.DeleteAsync(logs, false);
        }

        /// <summary>
        /// Clears a log
        /// </summary>
        /// <param name="olderThan">The date that sets the restriction on deleting records. Leave null to remove all records</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task ClearLogAsync(DateTime? olderThan = null)
        {
            if (olderThan == null)
                await _logRepository.TruncateAsync();
            else
                await _logRepository.DeleteAsync(p => p.CreatedOnUtc < olderThan.Value);
        }

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
        public virtual async Task<IPagedList<LogRecord>> GetAllLogsAsync(DateTime? fromUtc = null, DateTime? toUtc = null,
            string message = "", LogLevel? logLevel = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var logs = await _logRepository.GetAllPagedAsync(query =>
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
        /// <param name="logId">LogRecord item identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the log item
        /// </returns>
        public virtual async Task<LogRecord> GetLogByIdAsync(int logId)
        {
            return await _logRepository.GetByIdAsync(logId);
        }

        /// <summary>
        /// Get log items by identifiers
        /// </summary>
        /// <param name="logIds">LogRecord item identifiers</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the log items
        /// </returns>
        public virtual async Task<IList<LogRecord>> GetLogByIdsAsync(int[] logIds)
        {
            return await _logRepository.GetByIdsAsync(logIds);
        }

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
        public virtual async Task<LogRecord> InsertLogAsync(LogLevel logLevel, string shortMessage, string fullMessage = "")
        {
            //check ignore word/phrase list?
            if (IgnoreLog(shortMessage) || IgnoreLog(fullMessage))
                return null;

            var log = new LogRecord
            {
                LogLevel = logLevel,
                ShortMessage = shortMessage,
                FullMessage = fullMessage,
                //IpAddress = _nodeHelper.GetCurrentIpAddress(),
                //UserId = user?.Id,
                CreatedOnUtc = DateTime.UtcNow
            };

            await _logRepository.InsertAsync(log, false);

            return log;
        }

        /// <summary>
        /// Inserts a log item
        /// </summary>
        /// <param name="logLevel">LogRecord level</param>
        /// <param name="shortMessage">The short message</param>
        /// <param name="fullMessage">The full message</param>
        /// <returns>
        /// LogRecord item
        /// </returns>
        public virtual LogRecord InsertLog(LogLevel logLevel, string shortMessage, string fullMessage = "")
        {
            //check ignore word/phrase list?
            if (IgnoreLog(shortMessage) || IgnoreLog(fullMessage))
                return null;

            var log = new LogRecord
            {
                LogLevel = logLevel,
                ShortMessage = shortMessage,
                FullMessage = fullMessage,
                //IpAddress = _nodeHelper.GetCurrentIpAddress(),
                //UserId = user?.Id,
                CreatedOnUtc = DateTime.UtcNow
            };

            _logRepository.Insert(log, false);

            return log;
        }

        /// <summary>
        /// Information
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InformationAsync(string message, Exception exception = null)
        {
            //don't log thread abort exception
            if (exception is ThreadAbortException)
                return;

            if (IsEnabled(LogLevel.Information))
                await InsertLogAsync(LogLevel.Information, message, exception?.ToString() ?? string.Empty);
        }

        /// <summary>
        /// Information
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        public virtual void Information(string message, Exception exception = null)
        {
            //don't log thread abort exception
            if (exception is ThreadAbortException)
                return;

            if (IsEnabled(LogLevel.Information))
                InsertLog(LogLevel.Information, message, exception?.ToString() ?? string.Empty);
        }

        /// <summary>
        /// Warning
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task WarningAsync(string message, Exception exception = null)
        {
            //don't log thread abort exception
            if (exception is ThreadAbortException)
                return;

            if (IsEnabled(LogLevel.Warning))
                await InsertLogAsync(LogLevel.Warning, message, exception?.ToString() ?? string.Empty);
        }

        /// <summary>
        /// Warning
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        public virtual void Warning(string message, Exception exception = null)
        {
            //don't log thread abort exception
            if (exception is ThreadAbortException)
                return;

            if (IsEnabled(LogLevel.Warning))
                InsertLog(LogLevel.Warning, message, exception?.ToString() ?? string.Empty);
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task ErrorAsync(string message, Exception exception = null)
        {
            //don't log thread abort exception
            if (exception is ThreadAbortException)
                return;

            if (IsEnabled(LogLevel.Error))
                await InsertLogAsync(LogLevel.Error, message, exception?.ToString() ?? string.Empty);
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="exception">Exception</param>
        public virtual void Error(string message, Exception exception = null)
        {
            //don't log thread abort exception
            if (exception is ThreadAbortException)
                return;

            if (IsEnabled(LogLevel.Error))
                InsertLog(LogLevel.Error, message, exception?.ToString() ?? string.Empty);
        }

        #endregion
    }
}
