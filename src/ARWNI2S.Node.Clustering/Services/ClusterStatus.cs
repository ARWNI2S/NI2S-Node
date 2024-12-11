namespace ARWNI2S.Clustering.Services
{
    public struct ClusterStatus
    {
        public int OnlineNodes { get; set; }
        public int OfflineNodes { get; set; }
        public int NodesWithError { get; set; }

        public int SpinningUpNodes { get; set; }
        public int SpinningDownNodes { get; set; }
    }
}
