namespace ARWNI2S.Runtime.Clustering.Messages
{
    internal class QuorumResponse
    {
        public bool Vote { get; internal set; }
        public bool HasErrors { get; internal set; }
    }
}