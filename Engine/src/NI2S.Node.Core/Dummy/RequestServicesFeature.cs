using Microsoft.Extensions.DependencyInjection;
using System;

namespace NI2S.Node.Dummy
{
    internal class RequestServicesFeature : IServiceProvidersFeature
    {
        private DefaultDummyContext context;
        private IServiceScopeFactory serviceScopeFactory;

        public RequestServicesFeature(DefaultDummyContext context, IServiceScopeFactory serviceScopeFactory)
        {
            this.context = context;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public IServiceProvider RequestServices { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}