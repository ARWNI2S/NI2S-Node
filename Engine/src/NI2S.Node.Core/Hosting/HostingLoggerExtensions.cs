using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace NI2S.Node.Hosting
{
    internal static class HostingLoggerExtensions
    {
        public static void EngineError(this ILogger logger, Exception exception)
        {
            logger.EngineError(
                eventId: LoggerEventIds.EngineStartupException,
                message: "Engine startup exception",
                exception: exception);
        }

        public static void HostingStartupAssemblyError(this ILogger logger, Exception exception)
        {
            logger.EngineError(
                eventId: LoggerEventIds.HostingStartupAssemblyException,
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
