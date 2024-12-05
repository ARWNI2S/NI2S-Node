using ARWNI2S.Engine.Parts;

namespace ARWNI2S.Engine
{
    public interface IEnginePartManager
    {
        IList<IEnginePart> EngineParts { get; }
    }
}