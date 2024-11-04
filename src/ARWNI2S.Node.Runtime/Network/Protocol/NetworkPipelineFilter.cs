using SuperSocket.ProtoBase;
using System.Buffers;

namespace ARWNI2S.Runtime.Network.Protocol
{
    public class NetworkPipelineFilter : IPipelineFilter<NI2SPackageInfo>
    {
        public IPackageDecoder<NI2SPackageInfo> Decoder { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IPipelineFilter<NI2SPackageInfo> NextFilter => throw new NotImplementedException();

        public object Context { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public NI2SPackageInfo Filter(ref SequenceReader<byte> reader)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }

    public class SslNetworkPipelineFilter : NetworkPipelineFilter
    {
        public SslNetworkPipelineFilter() : base()
        {
            // Configura adicionalmente la seguridad para SSL
        }
    }

}
