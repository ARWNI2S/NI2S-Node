using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace NI2S.Node.Dummy
{
    internal class DefaultDummyResponse : DummyResponse
    {
        private DefaultDummyContext defaultDummyContext;

        public DefaultDummyResponse(DefaultDummyContext defaultDummyContext)
        {
            this.defaultDummyContext = defaultDummyContext;
        }

        public override DummyContext DummyContext => throw new NotImplementedException();

        public override int StatusCode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override IHeaderDictionary Headers => throw new NotImplementedException();

        public override Stream Body { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override long? ContentLength { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string ContentType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override bool HasStarted => throw new NotImplementedException();

        public override void OnCompleted(Func<object, Task> callback, object state)
        {
            throw new NotImplementedException();
        }

        public override void OnStarting(Func<object, Task> callback, object state)
        {
            throw new NotImplementedException();
        }

        public override void Redirect([StringSyntax("Uri")] string location, bool permanent)
        {
            throw new NotImplementedException();
        }

        internal void Initialize(int revision)
        {
            throw new NotImplementedException();
        }

        internal void Uninitialize()
        {
            throw new NotImplementedException();
        }
    }
}