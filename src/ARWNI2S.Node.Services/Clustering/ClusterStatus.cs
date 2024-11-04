namespace ARWNI2S.Node.Services.Clustering
{
    public struct ClusterStatus
    {
        public int OnlineNodes { get; internal set; }
        public int OfflineNodes { get; internal set; }
        public int NodesWithError { get; internal set; }

        public int SpinningUpNodes { get; internal set; }
        public int SpinningDownNodes { get; internal set; }
    }
}
