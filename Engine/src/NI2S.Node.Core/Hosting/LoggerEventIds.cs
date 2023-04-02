namespace NI2S.Node.Hosting
{
    internal static class LoggerEventIds
    {
        public const int RequestStarting = 1;
        public const int RequestFinished = 2;
        public const int Starting = 3;
        public const int Started = 4;
        public const int Shutdown = 5;
        public const int EngineStartupException = 6;
        public const int EngineStoppingException = 7;
        public const int EngineStoppedException = 8;
        public const int HostedServiceStartException = 9;
        public const int HostedServiceStopException = 10;
        public const int HostingStartupAssemblyException = 11;
        public const int ServerShutdownException = 12;
        public const int HostingStartupAssemblyLoaded = 13;
        public const int ServerListeningOnAddresses = 14;
    }
}