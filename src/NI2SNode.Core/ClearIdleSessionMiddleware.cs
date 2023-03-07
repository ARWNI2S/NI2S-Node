using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NI2S.Node.Configuration.Options;
using NI2S.Node.Protocol.Channel;

namespace NI2S.Node.Server
{
    class ClearIdleSessionMiddleware : MiddlewareBase
    {
        private readonly ISessionContainer? _sessionContainer;

        private Timer? _timer;

        private readonly ServerOptions _serverOptions;

        private readonly ILogger _logger;

        public ClearIdleSessionMiddleware(IServiceProvider serviceProvider, IOptions<ServerOptions> serverOptions, ILoggerFactory loggerFactory)
        {
            _sessionContainer = serviceProvider.GetService<ISessionContainer>();

            if (_sessionContainer == null)
                throw new Exception($"{nameof(ClearIdleSessionMiddleware)} needs a middleware of {nameof(ISessionContainer)}");

            _serverOptions = serverOptions.Value;
            _logger = loggerFactory.CreateLogger<ClearIdleSessionMiddleware>();
        }

        public override void Start(INode server)
        {
            _timer = new Timer(OnTimerCallback, null, _serverOptions.ClearIdleSessionInterval * 1000, _serverOptions.ClearIdleSessionInterval * 1000);
        }

        private void OnTimerCallback(object? state)
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);

            try
            {
                var timeoutTime = DateTimeOffset.Now.AddSeconds(0 - _serverOptions.IdleSessionTimeOut);

                if (_sessionContainer == null)
                    throw new Exception();

                foreach (var s in _sessionContainer.GetSessions())
                {
                    if (s.LastActiveTime <= timeoutTime)
                    {
                        try
                        {
                            s.Channel?.CloseAsync(CloseReason.TimeOut);
                            _logger.LogWarning($"Close the idle session {s.SessionID}, it's LastActiveTime is {s.LastActiveTime}.");
                        }
                        catch (Exception exc)
                        {
                            _logger.LogError(exc, $"Error happened when close the session {s.SessionID} for inactive for a while.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error happened when clear idle session.");
            }

            _timer?.Change(_serverOptions.ClearIdleSessionInterval * 1000, _serverOptions.ClearIdleSessionInterval * 1000);
        }

        public override void Shutdown(INode server)
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
            _timer?.Dispose();
            _timer = null;
        }
    }
}