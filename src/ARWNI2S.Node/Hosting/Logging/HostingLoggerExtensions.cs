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