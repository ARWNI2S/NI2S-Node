using ARWNI2S.Engine.Core.Object;

namespace ARWNI2S.Engine.Core.Internal.Kernel
{
    internal sealed class Dispatcher : IPureSingleton<Dispatcher>, IEngineProcessor
    {
        private readonly IUpdateManager _updateManager;
        private readonly Thread _workingThread;

        public Dispatcher(IUpdateManager updateManager, IThreadManager threadManager)
        {
            _updateManager = updateManager;
            _workingThread = threadManager.GetDedicatedInnerThread(this);
        }

        public static Dispatcher Instance
        {
            get
            {
                if (Singleton<Dispatcher>.Instance == null)
                {
                    var dispatcher = CreateInstance();
                    Singleton<Dispatcher>.Instance = dispatcher;
                }
                return Singleton<Dispatcher>.Instance;
            }
        }

        private static Dispatcher CreateInstance()
        {
            var updateManager = EngineContext.Current.Resolve<IUpdateManager>();
            var threadManager = EngineContext.Current.Resolve<IThreadManager>();


            return new Dispatcher(updateManager, threadManager);
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }

        //public void QueueEvent(INiisObject sender, INiisObject target, ) { }
    }
}
