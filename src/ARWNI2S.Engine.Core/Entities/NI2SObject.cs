
using ARWNI2S.Engine.Core;

namespace ARWNI2S.Entities
{
    public abstract class NI2SObject : IObjectEntity
    {
        public Guid UUID { get; private set; }

        public NI2SObject() : this(new ObjectInitializer()) { }

        protected NI2SObject(ObjectInitializer initializer)
        {
            initializer.Target = this;
        }


    }
}
