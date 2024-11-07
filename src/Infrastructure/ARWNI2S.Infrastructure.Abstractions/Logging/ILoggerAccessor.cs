using Microsoft.Extensions.Logging;

namespace ARWNI2S.Infrastructure.Logging
{
    public interface ILoggerAccessor
    {
        ILogger Logger { get; }
    }
}