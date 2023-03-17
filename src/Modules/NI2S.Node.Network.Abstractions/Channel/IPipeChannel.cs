using System.IO.Pipelines;

namespace NI2S.Node.Networking.Channel
{
    public interface IPipeChannel
    {
        Pipe In { get; }

        Pipe Out { get; }

        IPipelineFilter PipelineFilter { get; }
    }
}