using ARWNI2S.Engine;
using ARWNI2S.Engine.Extensibility;
using ARWNI2S.Engine.Hosting;

namespace ARWNI2S.Node.Builder
{
    internal class EngineBuilder : IEngineBuilder
    {
        public EngineBuilder(IServiceProvider services, IModuleCollection nodeModules)
        {
            EngineServices = services;
            NodeModules = nodeModules;
        }

        public IServiceProvider EngineServices { get; set; }

        public IDictionary<string, object> Properties { get; } = new Dictionary<string, object>();

        public IModuleCollection NodeModules { get; }

        //HACK
        public INiisEngine Build()
        {
            throw new NotImplementedException();
        }

        public IEngineBuilder New()
        {
            throw new NotImplementedException();
        }

        //public IEngineBuilder Use(Func<UpdateDelegate, UpdateDelegate> middleware)
        //{
        //    throw new NotImplementedException();
        //}
    }
}