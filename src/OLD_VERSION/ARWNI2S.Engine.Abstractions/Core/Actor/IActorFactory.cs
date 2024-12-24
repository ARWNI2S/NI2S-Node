using ARWNI2S.Engine.Core.Object;

namespace ARWNI2S.Engine.Core.Actor
{
    internal interface IActorFactory<TActor> : IObjectFactory<TActor> where TActor : INiisActor
    {
    }
}