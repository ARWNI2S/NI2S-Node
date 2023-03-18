using System;

namespace NI2S.Node.Diagnostics
{
    /// <summary>
    /// Annotation intended to publish status information. 
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class StatusInfoAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusInfoAttribute" /> class.
        /// </summary>
        public StatusInfoAttribute()
        {
            OutputInPerfLog = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusInfoAttribute" /> class.
        /// </summary>
        /// <param name="key">The status information key.</param>
        public StatusInfoAttribute(string key)
            : this()
        {
            Key = key;
        }

        /// <summary>
        /// Gets or sets the status information key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string? Key { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the short name.
        /// </summary>
        /// <value>
        /// The short name.
        /// </value>
        public string? ShortName { get; set; }

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>
        /// The format.
        /// </value>
        public string? Format { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether [output in perf log].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [output in perf log]; otherwise, <c>false</c>.
        /// </value>
        public bool OutputInPerfLog { get; set; }

        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>
        /// The type of the data.
        /// </value>
        public Type? DataType { get; set; }
    }

    /// <summary>
    /// Holds key names status information metadata.
    /// </summary>
    public class StatusInfoKeys
    {
        #region Shared

        /// <summary>
        /// The cpu usage
        /// </summary>
        public const string CpuUsage = "CpuUsage";

        /// <summary>
        /// The memory usage
        /// </summary>
        public const string MemoryUsage = "MemoryUsage";

        /// <summary>
        /// The total thread count
        /// </summary>
        public const string TotalThreadCount = "TotalThreadCount";

        /// <summary>
        /// The available working threads count
        /// </summary>
        public const string AvailableWorkingThreads = "AvailableWorkingThreads";

        /// <summary>
        /// The available completion port threads count
        /// </summary>
        public const string AvailableCompletionPortThreads = "AvailableCompletionPortThreads";

        /// <summary>
        /// The max working threads count
        /// </summary>
        public const string MaxWorkingThreads = "MaxWorkingThreads";

        /// <summary>
        /// The max completion port threads count
        /// </summary>
        public const string MaxCompletionPortThreads = "MaxCompletionPortThreads";

        #endregion

        #region For node engine instance

        /// <summary>
        /// The started time.
        /// </summary>
        public const string StartedTime = "StartedTime";

        /// <summary>
        /// <see langword="true"/> if this instance is running; otherwise, <see langword="false"/>.
        /// </summary>
        public const string IsRunning = "IsRunning";

        /// <summary>
        /// The total count of connections.
        /// </summary>
        public const string TotalConnections = "TotalConnections";

        /// <summary>
        /// The max connection number.
        /// </summary>
        public const string MaxConnectionNumber = "MaxConnectionNumber";

        /// <summary>
        /// The total handled message count.
        /// </summary>
        public const string TotalHandledMessages = "TotalHandledMessages";

        /// <summary>
        /// The message handling speed, per second.
        /// </summary>
        public const string MessageHandlingSpeed = "MessageHandlingSpeed";

        /// <summary>
        /// The total number of listeners.
        /// </summary>
        public const string NumListeners = "NumListeners";

        /// <summary>
        /// The available sending queue items.
        /// </summary>
        public const string AvialableSendingQueueItems = "AvialableSendingQueueItems";

        /// <summary>
        /// The total sending queue items.
        /// </summary>
        public const string TotalSendingQueueItems = "TotalSendingQueueItems";

        #endregion
    }
}
