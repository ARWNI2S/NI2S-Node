using System;

namespace NI2S.Node.Dummy
{
    public class DummyResponse
    {
        public int StatusCode { get; internal set; }
        public object ContentType { get; internal set; }
        public object ContentLength { get; internal set; }

        public object Headers { get; internal set; }
    }
}