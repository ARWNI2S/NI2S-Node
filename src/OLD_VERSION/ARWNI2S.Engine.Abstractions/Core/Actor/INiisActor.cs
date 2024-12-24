using ARWNI2S.Engine.Core.Object;

namespace ARWNI2S.Engine.Core.Actor
{
    public interface INiisActor : INiisObject
    {
        IEnumerable<IActorComponent> Components { get; }
    }
}