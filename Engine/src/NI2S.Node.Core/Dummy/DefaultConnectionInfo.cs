using Microsoft.AspNetCore.Http.Features;
using System;

namespace NI2S.Node.Dummy
{
    internal class DefaultConnectionInfo: ConnectionInfo
    {
        private IFeatureCollection features;

        public DefaultConnectionInfo(IFeatureCollection features)
        {
            this.features = features;
        }

        internal void Initialize(IFeatureCollection features, int revision)
        {
            throw new NotImplementedException();
        }

        internal void Uninitialize()
        {
            throw new NotImplementedException();
        }
    }
}