using NI2S.Node.Protocol.Session;
using System;

namespace NI2S.Node.Command
{
    public struct CommandExecutingContext
    {
        /// <summary>
        /// Gets the session.
        /// </summary>
        public ISession Session { get; set; }

        /// <summary>
        /// Gets the request info.
        /// </summary>
        public object Package { get; set; }

        /// <summary>
        /// Gets the current command.
        /// </summary>
        public ICommand CurrentCommand { get; set; }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception { get; set; }
    }
}
