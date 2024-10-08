using ARWNI2S.Node.Core.Infrastructure;

namespace ARWNI2S.Node.Core
{
    public class EngineContext
    {
        private IEngine _engineInstance;

        public IEngine GetCurent() { return _engineInstance; }
    }
}
