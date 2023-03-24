namespace NI2S.Node
{
    public sealed class EngineContext : IEngineContext
    {
        INodeEngine IEngineContext.GetEngine()
        {
            throw new System.NotImplementedException();
        }
    }
}
