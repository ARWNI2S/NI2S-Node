using ARWNI2S.Core.Entities;
using ARWNI2S.Entities;

namespace ARWNI2S.Core.Object
{
    internal class NiisObjectInitializer : IObjectInitializer
    {
        public NiisObject Target { get; internal set; }

        #region IObjectInitializer implementation

        IObjectEntity IObjectInitializer.Target { get => Target; set => Target = (NiisObject)value; }

        #endregion
    }
}