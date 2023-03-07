using Microsoft.Extensions.Logging;

namespace NI2S.Node
{
    public interface ILoggerAccessor
    {
        ILogger Logger { get; }
    }
}