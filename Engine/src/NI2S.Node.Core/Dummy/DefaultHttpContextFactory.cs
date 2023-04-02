using Microsoft.AspNetCore.Http.Features;
using System;

namespace NI2S.Node.Dummy
{
    internal class DefaultDummyContextFactory : IDummyContextFactory
    {
        public object DummyContextAccessor { get; internal set; }

        public void Dispose(DummyContext httpContext)
        {
            throw new NotImplementedException();
        }

        internal DummyContext Create(IFeatureCollection contextFeatures)
        {
            throw new NotImplementedException();
        }

        internal void Dispose(DefaultDummyContext httpContext)
        {
            throw new NotImplementedException();
        }

        internal void Initialize(DefaultDummyContext defaultDummyContext, IFeatureCollection contextFeatures)
        {
            throw new NotImplementedException();
        }

        DummyContext IDummyContextFactory.Create(IFeatureCollection contextFeatures)
        {
            throw new NotImplementedException();
        }
    }
}
