using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NI2S.Node.Configuration;
using NI2S.Node.Logging;
using NI2S.Node.Middleware;
using NI2S.Node.Protocol;
using NI2S.Node.Protocol.Channel;
using NI2S.Node.Protocol.Session;

namespace NI2S.Node
{
    public class NodeService<TReceivePackageInfo> : IHostedService, INode, IChannelRegister, ILoggerAccessor, ISessionEventHost
    {
        private readonly IServiceProvider _serviceProvider;

        public IServiceProvider ServiceProvider
        {
            get { return _serviceProvider; }
        }

        public ServerOptions Options { get; }
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;

        internal protected ILogger Logger
        {
            get { return _logger; }
        }

        ILogger ILoggerAccessor.Logger
        {
            get { return _logger; }
        }

        private readonly IPipelineFilterFactory<TReceivePackageInfo> _pipelineFilterFactory;
        private readonly IChannelCreatorFactory _channelCreatorFactory;
        private List<IChannelCreator>? _channelCreators;
        private readonly IPackageHandlingScheduler<TReceivePackageInfo>? _packageHandlingScheduler;
        private readonly IPackageHandlingContextAccessor<TReceivePackageInfo>? _packageHandlingContextAccessor;

        public string? Name { get; }

        private int _sessionCount;

        public int SessionCount => _sessionCount;

        private readonly ISessionFactory? _sessionFactory;

        private IMiddleware[]? _middlewares;

        protected IMiddleware[]? Middlewares
        {
            get { return _middlewares; }
        }

        private NodeState _state = NodeState.None;

        public NodeState State
        {
            get { return _state; }
        }

        public object? DataContext { get; set; }

        private readonly SessionHandlers? _sessionHandlers;

        public NodeService(IServiceProvider serviceProvider, IOptions<ServerOptions> serverOptions)
        {
            if (serverOptions == null)
                throw new ArgumentNullException(nameof(serverOptions));

            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            Name = serverOptions.Value.Name;
            Options = serverOptions.Value;
            _pipelineFilterFactory = GetPipelineFilterFactory();
            _loggerFactory = serviceProvider.GetService<ILoggerFactory>()!;
            _logger = _loggerFactory.CreateLogger("NodeService");
            _channelCreatorFactory = serviceProvider.GetService<IChannelCreatorFactory>() ?? new TcpChannelCreatorFactory(serviceProvider);
            _sessionHandlers = serviceProvider.GetService<SessionHandlers>();
            _sessionFactory = serviceProvider.GetService<ISessionFactory>();
            _packageHandlingContextAccessor = serviceProvider.GetService<IPackageHandlingContextAccessor<TReceivePackageInfo>>();
            InitializeMiddlewares();

            var packageHandler = serviceProvider.GetService<IPackageHandler<TReceivePackageInfo>>()
                ?? _middlewares?.OfType<IPackageHandler<TReceivePackageInfo>>().FirstOrDefault();

            if (packageHandler == null)
            {
                Logger.LogWarning("The PackageHandler cannot be found.");
            }
            else
            {
                var errorHandler = serviceProvider.GetService<Func<ISession, PackageHandlingException<TReceivePackageInfo>, ValueTask<bool>>>()
                ?? OnSessionErrorAsync;

                _packageHandlingScheduler = serviceProvider.GetService<IPackageHandlingScheduler<TReceivePackageInfo>>()
                    ?? new SerialPackageHandlingScheduler<TReceivePackageInfo>();
                _packageHandlingScheduler.Initialize(packageHandler, errorHandler);
            }
        }

        protected virtual IPipelineFilterFactory<TReceivePackageInfo> GetPipelineFilterFactory()
        {
            return _serviceProvider.GetRequiredService<IPipelineFilterFactory<TReceivePackageInfo>>();
        }

        private bool AddChannelCreator(ListenOptions? listenOptions, ServerOptions serverOptions)
        {
            var listener = _channelCreatorFactory.CreateChannelCreator<TReceivePackageInfo>(listenOptions, serverOptions, _loggerFactory, _pipelineFilterFactory);
            listener.NewClientAccepted += OnNewClientAccept;

            if (!listener.Start())
            {
                _logger.LogError($"Failed to listen {listener}.");
                return false;
            }

            _logger.LogInformation($"The listener [{listener}] has been started.");
            _channelCreators?.Add(listener);
            return true;
        }

        private Task<bool> StartListenAsync(CancellationToken cancellationToken)
        {
            _channelCreators = new List<IChannelCreator>();

            var serverOptions = Options;

            if (serverOptions.Listeners != null && serverOptions.Listeners.Any())
            {
                foreach (var l in serverOptions.Listeners)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    if (!AddChannelCreator(l, serverOptions))
                    {
                        continue;
                    }
                }
            }
            else
            {
                _logger.LogWarning("No listener was defined, so this server only can accept connections from the ActiveConnect.");

                if (!AddChannelCreator(null, serverOptions))
                {
                    _logger.LogError($"Failed to add the channel creator.");
                    return Task.FromResult(false);
                }
            }

            return Task.FromResult(_channelCreators.Any());
        }

        protected virtual void OnNewClientAccept(IChannelCreator listener, IChannel channel)
        {
            AcceptNewChannel(channel);
        }

        private void AcceptNewChannel(IChannel channel)
        {
            if (_sessionFactory == null) throw new InvalidOperationException();//TODO: Message

            var session = (Session)_sessionFactory.Create();
            HandleSession(session, channel).DoNotAwait();
        }

        async Task IChannelRegister.RegisterChannel(object connection)
        {
            var channel = await _channelCreators?.FirstOrDefault()?.CreateChannel(connection)!;
            AcceptNewChannel(channel);
        }

        protected virtual object CreatePipelineContext(ISession session)
        {
            return session;
        }

        #region Middlewares

        private void InitializeMiddlewares()
        {
            _middlewares = _serviceProvider.GetServices<IMiddleware>()
                .OrderBy(m => m.Order)
                .ToArray();

            foreach (var m in _middlewares)
            {
                m.Start(this);
            }
        }

        private void ShutdownMiddlewares()
        {
            if (_middlewares != null)
            {
                foreach (var m in _middlewares)
                {
                    try
                    {
                        m.Shutdown(this);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"The exception was thrown from the middleware {m.GetType().Name} when it is being shutdown.");
                    }
                }
            }
        }

        private async ValueTask<bool> RegisterSessionInMiddlewares(ISession session)
        {
            var middlewares = _middlewares;

            if (middlewares != null && middlewares.Length > 0)
            {
                for (var i = 0; i < middlewares.Length; i++)
                {
                    var middleware = middlewares[i];

                    if (!await middleware.RegisterSession(session))
                    {
                        _logger.LogWarning($"A session from {session.RemoteEndPoint} was rejected by the middleware {middleware.GetType().Name}.");
                        return false;
                    }
                }
            }

            return true;
        }

        private async ValueTask UnRegisterSessionFromMiddlewares(ISession session)
        {
            var middlewares = _middlewares;

            if (middlewares != null && middlewares.Length > 0)
            {
                for (var i = 0; i < middlewares.Length; i++)
                {
                    var middleware = middlewares[i];

                    try
                    {
                        if (!await middleware.UnRegisterSession(session))
                        {
                            _logger.LogWarning($"The session from {session.RemoteEndPoint} was failed to be unregistered from the middleware {middleware.GetType().Name}.");
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"An unhandled exception occured when the session from {session.RemoteEndPoint} was being unregistered from the middleware {nameof(RegisterSessionInMiddlewares)}.");
                    }
                }
            }
        }

        #endregion

        private async ValueTask<bool> InitializeSession(ISession session, IChannel channel)
        {
            session.Initialize(this, channel);

            if (channel is IPipeChannel pipeChannel)
            {
                pipeChannel.PipelineFilter.Context = CreatePipelineContext(session);
            }

            var middlewares = _middlewares;

            try
            {
                if (!await RegisterSessionInMiddlewares(session))
                    return false;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An unhandled exception occured in {nameof(RegisterSessionInMiddlewares)}.");
                return false;
            }

            channel.Closed += (s, e) => OnChannelClosed(session, e);
            return true;
        }


        protected virtual ValueTask OnSessionConnectedAsync(ISession session)
        {
            var connectedHandler = _sessionHandlers?.Connected;

            if (connectedHandler != null)
                return connectedHandler.Invoke(session);

            return new ValueTask();
        }

        private void OnChannelClosed(ISession session, CloseEventArgs e)
        {
            FireSessionClosedEvent((Session)session, e.Reason).DoNotAwait();
        }

        protected virtual ValueTask OnSessionClosedAsync(ISession session, CloseEventArgs e)
        {
            var closedHandler = _sessionHandlers?.Closed;

            if (closedHandler != null)
                return closedHandler.Invoke(session, e);

            return ValueTask.CompletedTask;
        }

        protected virtual async ValueTask FireSessionConnectedEvent(Session session)
        {
            if (session is IHandshakeRequiredSession handshakeSession)
            {
                if (!handshakeSession.Handshaked)
                    return;
            }

            _logger.LogInformation($"A new session connected: {session.SessionID}");

            try
            {
                Interlocked.Increment(ref _sessionCount);
                await session.FireSessionConnectedAsync();
                await OnSessionConnectedAsync(session);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "There is one exception thrown from the event handler of SessionConnected.");
            }
        }

        protected virtual async ValueTask FireSessionClosedEvent(Session session, CloseReason reason)
        {
            if (session is IHandshakeRequiredSession handshakeSession)
            {
                if (!handshakeSession.Handshaked)
                    return;
            }

            await UnRegisterSessionFromMiddlewares(session);

            _logger.LogInformation($"The session disconnected: {session.SessionID} ({reason})");

            try
            {
                Interlocked.Decrement(ref _sessionCount);

                var closeEventArgs = new CloseEventArgs(reason);
                await session.FireSessionClosedAsync(closeEventArgs);
                await OnSessionClosedAsync(session, closeEventArgs);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, "There is one exception thrown from the event of OnSessionClosed.");
            }
        }

        ValueTask ISessionEventHost.HandleSessionConnectedEvent(Session session)
        {
            return FireSessionConnectedEvent(session);
        }

        ValueTask ISessionEventHost.HandleSessionClosedEvent(Session session, CloseReason reason)
        {
            return FireSessionClosedEvent(session, reason);
        }

        private async ValueTask HandleSession(Session session, IChannel channel)
        {
            if (!await InitializeSession(session, channel))
                return;

            try
            {
                channel.Start();

                await FireSessionConnectedEvent(session);

                var packageChannel = (IChannel<TReceivePackageInfo>)channel;
                var packageHandlingScheduler = _packageHandlingScheduler;

                await foreach (var p in packageChannel.RunAsync())
                {
                    if (_packageHandlingContextAccessor != null)
                    {
                        _packageHandlingContextAccessor.PackageHandlingContext = new PackageHandlingContext<ISession, TReceivePackageInfo>(session, p);
                    }
                    if (packageHandlingScheduler != null)
                        await packageHandlingScheduler.HandlePackage(session, p);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to handle the session {session.SessionID}.");
            }
        }

        protected virtual ValueTask<bool> OnSessionErrorAsync(ISession session, PackageHandlingException<TReceivePackageInfo> exception)
        {
            _logger.LogError(exception, $"Session[{session.SessionID}]: session exception.");
            return new ValueTask<bool>(true);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var state = _state;

            if (state != NodeState.None && state != NodeState.Stopped)
            {
                throw new InvalidOperationException($"The server cannot be started right now, because its state is {state}.");
            }

            _state = NodeState.Starting;

            if (!await StartListenAsync(cancellationToken))
            {
                _state = NodeState.Failed;
                _logger.LogError("Failed to start any listener.");
                return;
            }

            _state = NodeState.Started;

            try
            {
                await OnStartedAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "There is one exception thrown from the method OnStartedAsync().");
            }
        }

        protected virtual ValueTask OnStartedAsync()
        {
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask OnStopAsync()
        {
            return ValueTask.CompletedTask;
        }

        private async Task StopListener(IChannelCreator listener)
        {
            await listener.StopAsync().ConfigureAwait(false);
            _logger.LogInformation($"The listener [{listener}] has been stopped.");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            var state = _state;

            if (state != NodeState.Started)
            {
                throw new InvalidOperationException($"The server cannot be stopped right now, because its state is {state}.");
            }

            _state = NodeState.Stopping;

            if (_channelCreators != null)
            {
                var tasks = _channelCreators.Where(l => l.IsRunning).Select(l => StopListener(l))
                .Union(new Task[] { Task.Run(ShutdownMiddlewares) });

                await Task.WhenAll(tasks).ConfigureAwait(false);
            }

            try
            {
                await OnStopAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "There is an exception thrown from the method OnStopAsync().");
            }
            _state = NodeState.Stopped;
        }

        async Task<bool> INode.StartAsync()
        {
            await StartAsync(CancellationToken.None);
            return true;
        }

        async Task INode.StopAsync()
        {
            await StopAsync(CancellationToken.None);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        ValueTask IAsyncDisposable.DisposeAsync() => DisposeAsync(true);

        protected virtual async ValueTask DisposeAsync(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    try
                    {
                        if (_state == NodeState.Started)
                        {
                            await StopAsync(CancellationToken.None);
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Failed to stop the server");
                    }
                }

                disposedValue = true;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            DisposeAsync(disposing).GetAwaiter().GetResult();
        }

        void IDisposable.Dispose()
        {
            DisposeAsync(true).GetAwaiter().GetResult();
        }

        #endregion

        public class SessionHandlers
        {
            public Func<ISession, ValueTask>? Connected { get; set; }

            public Func<ISession, CloseEventArgs, ValueTask>? Closed { get; set; }
        }
    }
}
