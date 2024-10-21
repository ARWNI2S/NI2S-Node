namespace ARWNI2S.Infrastructure.Engine
{
    public interface IEngineContext
    {
        IServiceProvider ContextServices { get; }
        string LocalHost { get; }
        ContextInfo Info { get; }
    }
}