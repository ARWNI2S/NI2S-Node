namespace ARWNI2S.Engine.Core
{
    internal interface IActorFactory<TActor> : IObjectFactory<TActor> where TActor : INiisActor
    {
    }
}