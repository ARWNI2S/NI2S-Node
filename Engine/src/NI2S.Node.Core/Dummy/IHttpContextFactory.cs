using Microsoft.AspNetCore.Http.Features;

namespace NI2S.Node.Dummy
{
    public interface IDummyContextFactory
    {
        DummyContext Create(IFeatureCollection contextFeatures);
        void Dispose(DummyContext httpContext);
    }
}