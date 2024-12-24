namespace ARWNI2S.Clustering.Configuration
{
    public class ClusterServerOptions
    {
        public string ClusterId { get; internal set; }
        public string ConnectionString { get; internal set; }
        public bool IsDevelopment { get; internal set; }
        public string KnownNodes { get; internal set; }
        public string ServiceId { get; internal set; }
        public SimulationClusteringType SiloStorageClustering { get; internal set; }
    }
}