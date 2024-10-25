using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ARWNI2S.Infrastructure.Logging
{
    internal sealed class TraceLoggingBuilder : ILoggingBuilder
    {
        public TraceLoggingBuilder(IServiceCollection services) { Services = services; }

        public IServiceCollection Services { get; }
    }
}
