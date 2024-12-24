using ARWNI2S.Extensibility;

namespace ARWNI2S.Engine.Builder
{
    public class EngineBuilder : IEngineBuilder
    {
        public EngineBuilder(IServiceProvider services, IModuleCollection nodeModules)
        {
        }

        public IServiceProvider EngineServices { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IModuleCollection NodeModules => throw new NotImplementedException();

        public IDictionary<string, object> Properties => throw new NotImplementedException();

        public INiisEngine Build()
        {
            throw new NotImplementedException();
        }

        public IEngineBuilder New()
        {
            throw new NotImplementedException();
        }
    }
}