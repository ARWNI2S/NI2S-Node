using SuperSocket.ProtoBase;

namespace ARWNI2S.Runtime.Network.Protocol
{
    public class RelayPipelineFilter : TerminatorPipelineFilter<NI2SPackageInfo>
    {
        public RelayPipelineFilter() : base(terminator) { }

        // Configuración para el protocolo simplificado
    }
}
