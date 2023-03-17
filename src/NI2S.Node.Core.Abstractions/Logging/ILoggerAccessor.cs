using Microsoft.Extensions.Logging;

namespace NI2S.Node.Logging
{
    public interface ILoggerAccessor
    {
        ILogger Logger { get; }
    }
}
