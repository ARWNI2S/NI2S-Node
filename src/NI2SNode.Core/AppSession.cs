using Microsoft.Extensions.Logging;
using NI2S.Node;
using NI2S.Node.Async;
using NI2S.Node.Protocol;
using NI2S.Node.Protocol.Channel;
using NI2S.Node.Protocol.Session;
using System.Net;

namespace NI2S.Node.Server
{
    public class AppSession : ISession, ILogger, ILoggerAccessor
    {
        private IChannel? _channel;

        protected internal IChannel? Channel
        {
            get { return _channel; }
        }

        public AppSession()
        {

        }

        void ISession.Initialize(INodeInfo server, IChannel channel)
        {
            if (channel is IChannelWithSessionIdentifier channelWithSessionIdentifier)
                SessionID = channelWithSessionIdentifier.SessionIdentifier;
            else
                SessionID = Guid.NewGuid().ToString();

            Server = server;
            StartTime = DateTimeOffset.Now;
            _channel = channel;
            State = SessionState.Initialized;
        }

        public string SessionID { get; private set; } = string.Empty;

        public DateTimeOffset StartTime { get; private set; }

        public SessionState State { get; private set; } = SessionState.None;

        public INodeInfo? Server { get; private set; }

        IChannel? ISession.Channel
        {
            get { return _channel; }
        }

        public object? DataContext { get; set; }

        public EndPoint? RemoteEndPoint
        {
            get { return _channel?.RemoteEndPoint; }
        }

        public EndPoint? LocalEndPoint
        {
            get { return _channel?.LocalEndPoint; }
        }

        public DateTimeOffset LastActiveTime
        {
            get { return _channel?.LastActiveTime ?? DateTimeOffset.MinValue; }
        }

        public event AsyncEventHandler? Connected;

        public event AsyncEventHandler<CloseEventArgs>? Closed;

        private Dictionary<object, object?> _items = default!;

        public object? this[object name]
        {
            get
            {
                var items = _items;

                if (items == null)
                    return null;


                if (items.TryGetValue(name, out object? value))
                    return value;

                return null;
            }

            set
            {
                lock (this)
                {
                    var items = _items ??= new Dictionary<object, object?>();
                    items[name] = value;
                }
            }
        }

        protected virtual ValueTask OnSessionClosedAsync(CloseEventArgs e)
        {
            return new ValueTask();
        }

        internal async ValueTask FireSessionClosedAsync(CloseEventArgs e)
        {
            State = SessionState.Closed;

            await OnSessionClosedAsync(e);

            var closeEventHandler = Closed;

            if (closeEventHandler == null)
                return;

            await closeEventHandler.Invoke(this, e);
        }


        protected virtual ValueTask OnSessionConnectedAsync()
        {
            return new ValueTask();
        }

        internal async ValueTask FireSessionConnectedAsync()
        {
            State = SessionState.Connected;

            await OnSessionConnectedAsync();

            var connectedEventHandler = Connected;

            if (connectedEventHandler == null)
                return;

            await connectedEventHandler.Invoke(this, EventArgs.Empty);
        }

        ValueTask ISession.SendAsync(ReadOnlyMemory<byte> data)
        {
            if (_channel == null) throw new InvalidOperationException(); //TODO: Message

            return _channel.SendAsync(data);
        }

        ValueTask ISession.SendAsync<TPackage>(IPackageEncoder<TPackage> packageEncoder, TPackage package)
        {
            if (_channel == null) throw new InvalidOperationException(); //TODO: Message

            return _channel.SendAsync(packageEncoder, package);
        }

        void ISession.Reset()
        {
            ClearEvent(ref Connected);
            ClearEvent(ref Closed);
            _items?.Clear();
            State = SessionState.None;
            _channel = null;
            DataContext = null;
            StartTime = default;
            Server = null;

            Reset();
        }

        protected virtual void Reset()
        {

        }

        private static void ClearEvent<TEventHandler>(ref TEventHandler? sessionEvent)
            where TEventHandler : Delegate
        {
            if (sessionEvent == null)
                return;

            foreach (var handler in sessionEvent.GetInvocationList())
            {
                sessionEvent = Delegate.Remove(sessionEvent, handler) as TEventHandler;
            }
        }

        public virtual async ValueTask CloseAsync()
        {
            await CloseAsync(CloseReason.LocalClosing);
        }

        public virtual async ValueTask CloseAsync(CloseReason reason)
        {
            var channel = Channel;

            if (channel == null)
                return;

            try
            {
                await channel.CloseAsync(reason);
            }
            catch
            {
            }
        }

        #region ILogger

        ILogger GetLogger()
        {
            return ((ILoggerAccessor)Server!).Logger;
        }

        void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            GetLogger().Log<TState>(logLevel, eventId, state, exception, (s, e) =>
            {
                return $"Session[{this.SessionID}]: {formatter(s, e)}";
            });
        }

        bool ILogger.IsEnabled(LogLevel logLevel)
        {
            return GetLogger().IsEnabled(logLevel);
        }

        IDisposable ILogger.BeginScope<TState>(TState state)
        {
            return GetLogger().BeginScope(state)!;
        }

        public ILogger Logger => this as ILogger;

        #endregion
    }
}