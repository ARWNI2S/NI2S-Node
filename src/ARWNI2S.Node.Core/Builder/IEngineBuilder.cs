using ARWNI2S.Node.Infrastructure;

namespace ARWNI2S.Node.Builder
{
    public interface IEngineBuilder
    {
        IServiceProvider EngineServices { get; }

        IEngineModules EngineModules { get; }

        IDictionary<string, object> Properties { get; }

        IEngine Build();

        IEngineBuilder New();

        IEngineBuilder Register(IEngineModules module);

    }
}
