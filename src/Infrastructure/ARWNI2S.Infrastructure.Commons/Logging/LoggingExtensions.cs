using Microsoft.Extensions.Logging;

namespace ARWNI2S.Infrastructure.Logging
{
    internal static class LoggingExtensions
    {
        #region Public log methods using ErrorCode categorization.

        public static void LogDebug(this ILogger logger, ErrorCode errorCode, string format, params object[] args)
        {
            logger.LogDebug((int)errorCode, format, args);
        }
        public static void LogInformation(this ILogger logger, ErrorCode errorCode, string format, params object[] args)
        {
            logger.LogInformation((int)errorCode, format, args);
        }
        public static void LogWarning(this ILogger logger, ErrorCode errorCode, string format, params object[] args)
        {
            logger.LogWarning((int)errorCode, format, args);
        }
        public static void LogWarning(this ILogger logger, ErrorCode errorCode, string message, Exception exception)
        {
            logger.LogWarning((int)errorCode, exception, message);
        }
        public static void LogError(this ILogger logger, ErrorCode errorCode, string message, Exception exception = null)
        {
            logger.LogError((int)errorCode, exception, message);
        }

        #endregion

    }
}
