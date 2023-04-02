using System;
using System.IO;

namespace NI2S.Node.Dummy
{
    internal class DefaultDummyRequest : DummyRequest
    {
        private DefaultDummyContext defaultDummyContext;

        public DefaultDummyRequest(DefaultDummyContext defaultDummyContext)
        {
            this.defaultDummyContext = defaultDummyContext;
        }

        public override DummyContext DummyContext => throw new NotImplementedException();

        public override string Method { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Scheme { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool IsDummys { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override HostString Host { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override PathString PathBase { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override PathString Path { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override QueryString QueryString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Protocol { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override IHeaderDictionary Headers => throw new NotImplementedException();

        public override long? ContentLength { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string ContentType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override Stream Body { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override bool HasFormContentType => throw new NotImplementedException();

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