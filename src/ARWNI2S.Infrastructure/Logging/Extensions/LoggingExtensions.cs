using ARWNI2S.Diagnostics;
using Microsoft.Extensions.Logging;

namespace ARWNI2S.Logging.Extensions
{
    public static class LoggingExtensions
    {
        #region Public log methods using LogCode categorization.

        public static void LogDebug(this ILogger logger, LogCode code, string format, params object[] args)
        {
            logger.LogDebug((int)code, format, args);
        }
        public static void LogInformation(this ILogger logger, LogCode code, string format, params object[] args)
        {
            logger.LogInformation((int)code, format, args);
        }
        public static void LogWarning(this ILogger logger, LogCode code, string format, params object[] args)
        {
            logger.LogWarning((int)code, format, args);
        }
        public static void LogWarning(this ILogger logger, LogCode code, string message, Exception exception)
        {
            logger.LogWarning((int)code, exception, message);
        }
        public static void LogError(this ILogger logger, LogCode code, string message, Exception exception = null)
        {
            logger.LogError((int)code, exception, message);
        }
        public static void LogCritical(this ILogger logger, LogCode code, string message, Exception exception = null)
        {
            logger.LogCritical((int)code, exception, message);
        }
        public static void LogTrace(this ILogger logger, LogCode code, Exception exception, string message, params object[] args)
        {
            logger.LogTrace((int)code, exception, message, args);
        }
        public static void LogTrace(this ILogger logger, LogCode code, string message, params object[] args)
        {
            logger.LogTrace((int)code, message, args);
        }


        #endregion

    }
}
