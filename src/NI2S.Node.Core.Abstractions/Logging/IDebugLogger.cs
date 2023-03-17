using Microsoft.Extensions.Logging;

namespace NI2S.Node.Logging
{
    public interface IDebugLogger : ILogger
    {
        DebugLevel Verbosity { get; }
    }
}
