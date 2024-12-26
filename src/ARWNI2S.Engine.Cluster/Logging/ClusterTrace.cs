using Microsoft.Extensions.Logging;

namespace ARWNI2S.Engine.Cluster.Logging
{
    internal sealed partial class ClusterTrace : ILogger
    {
        private readonly ILogger _generalLogger;
        private readonly ILogger _badRequestsLogger;
        private readonly ILogger _connectionsLogger;
        private readonly ILogger _http2Logger;
        private readonly ILogger _http3Logger;

        public ClusterTrace(ILoggerFactory loggerFactory)
        {
            _generalLogger = loggerFactory.CreateLogger("ARWNI2S.Engine.Cluster");
            _badRequestsLogger = loggerFactory.CreateLogger("ARWNI2S.Engine.Cluster.BadRequests");
            _connectionsLogger = loggerFactory.CreateLogger("ARWNI2S.Engine.Cluster.Connections");
            _http2Logger = loggerFactory.CreateLogger("ARWNI2S.Engine.Cluster.Http2");
            _http3Logger = loggerFactory.CreateLogger("ARWNI2S.Engine.Cluster.Http3");
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            => _generalLogger.Log(logLevel, eventId, state, exception, formatter);

        public bool IsEnabled(LogLevel logLevel) => _generalLogger.IsEnabled(logLevel);

        public IDisposable BeginScope<TState>(TState state) where TState : notnull => _generalLogger.BeginScope(state);
    }
}