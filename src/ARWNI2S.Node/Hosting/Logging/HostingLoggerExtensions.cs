﻿using Microsoft.Extensions.Logging;

namespace ARWNI2S.Engine.Diagnostics
{
    internal static class HostingLoggerExtensions
    {
        public static void EngineError(this ILogger logger, Exception exception)
        {
            if (exception == null)
                logger.LogError("Unknown engine error");
            else
            {
                logger.LogError($"Error: {exception}");
            }
        }
    }
}