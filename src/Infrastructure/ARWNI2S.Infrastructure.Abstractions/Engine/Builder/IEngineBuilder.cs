namespace ARWNI2S.Infrastructure.Engine.Builder
{
    public interface IEngineBuilder
    {
        IServiceProvider EngineServices { get; set; }

        IDictionary<string, object> Properties { get; }

        FrameDelegate Build();

        IEngineBuilder New();

        IEngineBuilder Use(Func<FrameDelegate, FrameDelegate> middleware);
    }
}
