namespace ARWNI2S.Infrastructure.Engine
{
    public interface IEngineContextAccessor
    {
        ExecutionContext EngineContext { get; }
    }
}
