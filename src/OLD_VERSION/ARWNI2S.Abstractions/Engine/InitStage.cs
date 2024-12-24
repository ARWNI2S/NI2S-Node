namespace ARWNI2S.Engine
{
    public class InitStage
    {
        public const int FirstOne = -1;
        public const int Early = 0;
        public const int DbInit = Early + 10;
        public const int Cluster = 100;
        public const int ClusterInit = Cluster;
        public const int Common = 100;
        public const int Initialization = Common;
        public const int Later = 99999;
        public const int Delayed = LastOne - 1;
        public const int LastOne = int.MaxValue;
    }
}
