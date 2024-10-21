namespace ARWNI2S.Infrastructure.Engine
{
    public interface IEngineContextAccessor
    {
        IEngineContext EngineContext { get; set; }
    }
}
