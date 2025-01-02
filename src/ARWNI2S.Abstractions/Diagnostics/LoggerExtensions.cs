using ARWNI2S.Diagnostics;
using Microsoft.Extensions.Logging;

namespace ARWNI2S.Diagnostics
{
    internal static class LoggerExtensions
    {
        public static void LogTrace(this ILogger logger, EventCode code, string message, params object[] args)
        {
            logger.LogTrace((int)code, message, args);
        }

        public static void LogDebug(this ILogger logger, EventCode code, string message, params object[] args)
        {
            logger.LogDebug((int)code, message, args);
        }

        //public static void LogInformation(this ILogger logger, EventCode code, string message, params object[] args)
        //{
        //    logger.LogInformation((int)code, message, args);
        //}

        //public static void LogWarning(this ILogger logger, EventCode code, string message, params object[] args)
        //{
        //    logger.LogWarning((int)code, message, args);
        //}

        public static void LogError(this ILogger logger, ErrorCode code, string message, params object[] args)
        {
            logger.LogError((int)code, message, args);
        }

        //public static void LogCritical(this ILogger logger, EventCode code, string message, params object[] args)
        //{
        //    logger.LogCritical((int)code, message, args);
        //}

        //public static void LogTrace(this ILogger logger, EventCode code, Exception exception, string message, params object[] args)
        //{
        //    logger.LogTrace((int)code, exception, message, args);
        //}

        //public static void LogDebug(this ILogger logger, EventCode code, Exception exception, string message, params object[] args)
        //{
        //    logger.LogDebug((int)code, exception, message, args);
        //}

        //public static void LogInformation(this ILogger logger, EventCode code, Exception exception, string message, params object[] args)
        //{
        //    logger.LogInformation((int)code, exception, message, args);
        //}

        //public static void LogWarning(this ILogger logger, EventCode code, Exception exception, string message, params object[] args)
        //{
        //    logger.LogWarning((int)code, exception, message, args);
        //}

        public static void LogError(this ILogger logger, ErrorCode code, Exception exception, string message, params object[] args)
        {
            logger.LogError((int)code, exception, message, args);
        }
    }
}
