// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.


using Microsoft.Extensions.Logging;
using NI2S.Node.Diagnostics;
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

        public static void PortsOverridenByUrls(this ILogger logger, string httpPorts, string httpsPorts, string urls)
        {
            logger.LogWarning(eventId: LoggerEventIds.PortsOverridenByUrls,
                message: "Overriding HTTP_PORTS '{http}' and HTTPS_PORTS '{https}'. Binding to values defined by URLS instead '{urls}'.",
                httpPorts, httpsPorts, urls);
        }
    }
}
