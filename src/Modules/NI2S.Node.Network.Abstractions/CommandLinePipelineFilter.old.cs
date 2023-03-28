namespace NI2S.Node.Networking
{
    public class CommandLinePipelineFilter : TerminatorPipelineFilter<StringPackageInfo>
    {
        public CommandLinePipelineFilter()
            : base(new[] { (byte)'\r', (byte)'\n' })
        {

        }
    }
}
