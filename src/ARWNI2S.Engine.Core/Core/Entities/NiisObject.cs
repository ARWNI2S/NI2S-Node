using ARWNI2S.Core.Object;
using ARWNI2S.Entities;
using ARWNI2S.Infrastructure;

namespace ARWNI2S.Core.Entities
{
    public abstract class NiisObject : IObjectEntity
    {
        private Guid uuid;

        protected NiisObject()
            : this(EngineActivator.CreateInstance<NiisObjectInitializer>())
        {
        }

        protected NiisObject(IObjectInitializer objectInitializer)
        {
            objectInitializer.Target = this;
        }

        public Guid UUID => uuid;
    }
}
