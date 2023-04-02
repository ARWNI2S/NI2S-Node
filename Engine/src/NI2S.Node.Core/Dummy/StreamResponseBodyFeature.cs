using System.IO;

namespace NI2S.Node.Dummy
{
    internal class StreamResponseBodyFeature : IDummyResponseBodyFeature
    {
        private Stream @null;

        public StreamResponseBodyFeature(Stream @null)
        {
            this.@null = @null;
        }
    }
}