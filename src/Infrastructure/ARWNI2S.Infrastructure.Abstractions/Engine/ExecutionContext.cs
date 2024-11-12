using ARWNI2S.Infrastructure.Network.Connection;
using ARWNI2S.Infrastructure.Session;
using System.Security.Claims;

namespace ARWNI2S.Infrastructure.Engine
{
    /// <summary>
    /// Encapsulates all game-specific information about an individual game events.
    /// </summary>
    public abstract class ExecutionContext
    {
        ///// <summary>
        ///// Gets the collection of engine features provided by the server and middleware available for this event.
        ///// </summary>
        //public abstract IFeatureCollection Features { get; }

        /// <summary>
        /// Gets the <see cref="IEvent"/> object for this message.
        /// </summary>
        public abstract IEvent Event { get; }

        /// <summary>
        /// Gets information about the underlying connection for this message.
        /// </summary>
        public abstract IConnection Connection { get; }

        /// <summary>
        /// Gets or sets the user for this message.
        /// </summary>
        public abstract ClaimsPrincipal User { get; set; }

        /// <summary>
        /// Gets or sets a key/value collection that can be used to share data within the scope of this message.
        /// </summary>
        public abstract IDictionary<object, object> Items { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IServiceProvider"/> that provides access to the message's service container.
        /// </summary>
        public abstract IServiceProvider RequestServices { get; set; }

        /// <summary>
        /// Notifies when the connection underlying this message is aborted and thus message operations should be
        /// cancelled.
        /// </summary>
        public abstract CancellationToken RequestAborted { get; set; }

        /// <summary>
        /// Gets or sets a unique identifier to represent this message in trace logs.
        /// </summary>
        public abstract string TraceIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the object used to manage user session data for this message.
        /// </summary>
        public abstract ISession Session { get; set; }

        /// <summary>
        /// Aborts the connection underlying this message.
        /// </summary>
        public abstract void Abort();
    }
}