using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using NI2S.Node.Hosting.Server;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NI2S.Node.Dummy
{
    internal class HostingApplication : IDummyApplication<DummyContext>
    {
        private RequestDelegate application;
        private ILogger logger;
        private DiagnosticListener diagnosticListener;
        private ActivitySource activitySource;
        private DistributedContextPropagator propagator;
        private IDummyContextFactory httpContextFactory;

        public HostingApplication(RequestDelegate application, ILogger logger, DiagnosticListener diagnosticListener, ActivitySource activitySource, DistributedContextPropagator propagator, IDummyContextFactory httpContextFactory)
        {
            this.application = application;
            this.logger = logger;
            this.diagnosticListener = diagnosticListener;
            this.activitySource = activitySource;
            this.propagator = propagator;
            this.httpContextFactory = httpContextFactory;
        }

        public DummyContext CreateContext(IFeatureCollection contextFeatures)
        {
            throw new NotImplementedException();
        }

        public void DisposeContext(DummyContext context, Exception exception)
        {
            throw new NotImplementedException();
        }

        public Task ProcessRequestAsync(DummyContext context)
        {
            throw new NotImplementedException();
        }
    }
}