using NI2S.Node.Engine.Modules;
using System;

namespace NI2S.Node.Dummy
{
    public abstract class DummyContext
    {
        /// <summary>
        /// Gets or sets the <see cref="IServiceProvider"/> that provides access to the dummy service container.
        /// </summary>
        public IServiceProvider DummyServices { get; set; }
        public DummyResponse Response { get; internal set; }
        public string TraceIdentifier { get; internal set; }
        public IDummyFeatureCollection Features { get; internal set; }

        public DummyRequest Request { get; internal set; }

        internal DummyEndpoint GetEndpoint()
        {
            throw new NotImplementedException();
        }
    }
}