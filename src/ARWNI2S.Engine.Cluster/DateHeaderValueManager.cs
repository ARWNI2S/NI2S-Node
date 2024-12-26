using ARWNI2S.Engine.Cluster.Environment;

namespace ARWNI2S.Engine.Cluster
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