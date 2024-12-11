using ARWNI2S.Engine.Features;

namespace ARWNI2S.Engine.Builder
{
    internal class EngineBuilder : IEngineBuilder
    {
        public EngineBuilder(IServiceProvider services, IFeatureCollection nodeFeatures)
        {
            EngineServices = services;
            NodeFeatures = nodeFeatures;
        }

        public IServiceProvider EngineServices { get; set; }

        public IDictionary<string, object> Properties { get; } = new Dictionary<string, object>();

        public IFeatureCollection NodeFeatures { get; }

        public INiisEngine Build()
        {
            throw new NotImplementedException();
        }

        public IEngineBuilder New()
        {
            throw new NotImplementedException();
        }

        public IEngineBuilder Use(Func<UpdateDelegate, UpdateDelegate> middleware)
        {
            throw new NotImplementedException();
        }
    }
}