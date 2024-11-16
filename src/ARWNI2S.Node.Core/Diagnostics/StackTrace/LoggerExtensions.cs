using Microsoft.Extensions.Logging;

namespace ARWNI2S.Node.Core.Diagnostics.StackTrace
{
    internal static partial class LoggerExtensions
    {
        [LoggerMessage(0, LogLevel.Debug, "Failed to read stack trace information for exception.", EventName = "FailedToReadStackTraceInfo")]
        public static partial void FailedToReadStackTraceInfo(this ILogger logger, Exception exception);
    }
}
