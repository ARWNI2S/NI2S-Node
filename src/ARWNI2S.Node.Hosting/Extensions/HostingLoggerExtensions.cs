using ARWNI2S.Infrastructure.Logging;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace ARWNI2S.Node.Hosting.Extensions
{
    internal static class HostingLoggerExtensions
    {
        public static void HostingStartupAssemblyError(this ILogger logger, Exception exception)
        {
            logger.EngineError(
                errorCode: ErrorCode.HostingStartupAssemblyException,
                message: "Hosting startup assembly exception",
                exception: exception);
        }

        public static void EngineError(this ILogger logger, Exception exception)
        {
            logger.EngineError(
                errorCode: ErrorCode.EngineStartupException,
                message: "Engine startup exception",
                exception: exception);
        }

        public static void EngineError(this ILogger logger, ErrorCode errorCode, string message, Exception exception)
        {
            logger.EngineError(
                eventId: (int)errorCode,
                message: "Hosting startup assembly exception",
                exception: exception);
        }

        public static void EngineError(this ILogger logger, EventId eventId, string message, Exception exception)
        {
            if (exception is ReflectionTypeLoadException reflectionTypeLoadException)
            {
                foreach (var ex in reflectionTypeLoadException.LoaderExceptions)
                {
                    if (ex != null)
                    {
                        message = message + Environment.NewLine + ex.Message;
                    }
                }
            }

            logger.LogCritical(
                eventId: eventId,
                message: message,
                exception: exception);
        }
    }
}
