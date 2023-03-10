using System.IO.Pipelines;

namespace NI2S.Node.Protocol.Channel
{
    public interface IPipeChannel
    {
        Pipe In { get; }

        Pipe Out { get; }

        IPipelineFilter PipelineFilter { get; }
    }
}