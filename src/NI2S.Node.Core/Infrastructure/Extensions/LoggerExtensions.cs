﻿using Microsoft.Extensions.Logging;
using NI2S.Node.Hosting.Internal;
using System;
using System.Reflection;

namespace NI2S.Node.Infrastructure
{
    internal static class LoggerExtensions
    {
        public static void ApplicationError(this ILogger logger, Exception exception)
        {
            logger.ApplicationError(
                eventId: LoggerEventIds.ApplicationStartupException,
                message: "Application startup exception",
                exception: exception);
        }

        public static void HostingStartupAssemblyError(this ILogger logger, Exception exception)
        {
            logger.ApplicationError(
                eventId: LoggerEventIds.HostingStartupAssemblyException,
                message: "Hosting startup assembly exception",
                exception: exception);
        }

        public static void ApplicationError(this ILogger logger, EventId eventId, string message, Exception exception)
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
