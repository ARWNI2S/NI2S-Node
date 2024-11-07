namespace ARWNI2S.Infrastructure.Network.Protocol
{
    public class CommandLinePipelineFilter : TerminatorPipelineFilter<StringPackageInfo>
    {
        public CommandLinePipelineFilter()
            : base(new[] { (byte)'\r', (byte)'\n' })
        {

        }
    }
}
