namespace ARWNI2S.Infrastructure.Logging
{
    ///// <summary>
    ///// 
    ///// </summary>
    //public enum LogLevel
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    Off = 0,
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    Error = 1,
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    Warning = 2,
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    Info = 3,
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    Verbose = 4,
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    Verbose2 = Verbose + 1,
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    Verbose3 = Verbose + 2
    //}

    /// <summary>
    /// The TraceLogger class distinguishes between five categories of loggers:
    /// </summary>
    public enum LoggerType
    {
        /// <summary>
        /// Logs that are written by the NI2S Host. 
        /// </summary>
        Host,
        /// <summary>
        /// Logs that are written by the NI2S run-time itself.
        /// This category should not be used by application code. 
        /// </summary>
        Runtime,
        /// <summary>
        /// Logs that are written by node entities.
        /// This category should be used by code that runs as NI2S entities in a node. 
        /// </summary>
        NodeEntity,
        /// <summary>
        /// Logs that are written by the scene application.
        /// This category should be used by node client-side application code. 
        /// </summary>
        Application,
        /// <summary>
        /// Logs that are written by the node providers.
        /// This category should be used by node providers. 
        /// </summary>
        Provider
    }
}
