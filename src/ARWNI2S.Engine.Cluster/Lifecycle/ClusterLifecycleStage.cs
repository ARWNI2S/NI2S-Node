namespace ARWNI2S.Cluster.Lifecycle
{
    /// <summary>
    /// Lifecycle stages of a NI2S node engine.
    /// </summary>
    public static class ClusterLifecycleStage
    {
        /// <summary>
        /// First valid stage in system's lifecycle
        /// </summary>
        public const int First = int.MinValue;

        /// <summary>
        /// Early host initialization
        /// </summary>
        public const int PreNodeInitialize = -1;

        /// <summary>
        /// Host services
        /// </summary>
        public const int NodeInitialize = 100;

        /// <summary>
        /// Later host initialization
        /// </summary>
        public const int PostNodeInitialize = 1000;

        /// <summary>
        /// Start network services
        /// </summary>
        public const int NetworkServices = 2000;

        /// <summary>
        /// Start cluster services
        /// </summary>
        public const int ClusterServices = 4000;

        /// <summary>
        /// Initialize clustered storage
        /// </summary>
        public const int StorageServices = 6000;

        /// <summary>
        /// Start node services
        /// </summary>
        public const int NodeServices = 8000;

        /// <summary>
        /// After node services have started.
        /// </summary>
        public const int AfterNodeServices = 8100;

        /// <summary>
        /// Service will be active after this step.
        /// It should only be used by the membership oracle 
        /// and the gateway, no other component should run
        /// at this stage
        /// </summary>
        public const int BecomeReady = Ready - 1;

        /// <summary>
        /// Service is active.
        /// </summary>
        public const int Ready = 10000;

        /// <summary>
        /// Last valid stage in service's lifecycle
        /// </summary>
        public const int Last = int.MaxValue;
    }
}
