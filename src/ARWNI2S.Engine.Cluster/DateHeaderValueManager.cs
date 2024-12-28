using ARWNI2S.Cluster.Environment;

namespace ARWNI2S.Cluster
{
    internal class DateHeaderValueManager : IHeartbeatHandler
    {
        private TimeProvider system;

        public DateHeaderValueManager(TimeProvider system)
        {
            this.system = system;
        }

        public void OnHeartbeat()
        {
            throw new NotImplementedException();
        }
    }
}