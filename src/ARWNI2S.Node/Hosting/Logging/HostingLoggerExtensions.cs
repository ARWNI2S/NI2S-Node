// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Microsoft.Extensions.Logging;

namespace ARWNI2S.Node.Hosting.Logging
{
    internal static class HostingLoggerExtensions
    {
        internal static void EngineError(this ILogger logger, Exception exception)
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