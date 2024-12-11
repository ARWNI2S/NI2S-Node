using ARWNI2S.Entities;

namespace ARWNI2S.Engine.Core
{
    public sealed class ObjectInitializer
    {
        public ObjectInitializer()
        {
        }

        public NI2SObject Target { get; internal set; }
    }
}