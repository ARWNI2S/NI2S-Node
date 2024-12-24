namespace ARWNI2S.Engine.Core.Actor
{
    public class DefaultActorFactory<TActor> : IActorFactory<TActor> where TActor : INiisActor
    {
        public TActor CreateInstance()
        {
            return default;
        }
    }
}
