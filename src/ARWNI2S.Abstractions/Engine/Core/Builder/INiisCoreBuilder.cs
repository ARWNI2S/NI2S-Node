using ARWNI2S.Engine.Parts;

namespace ARWNI2S.Engine.Core.Builder
{
    public interface INiisCoreBuilder
    {
        IEnginePartManager PartManager { get; }
    }
}