using NI2S.Node.Configuration.Options;
using System;

namespace NI2S.Node
{
    public interface INodeContext
    {
        string Name { get; }

        NodeOptions Options { get; }

        object DataContext { get; set; }

        //int SessionCount { get; }

        IServiceProvider ServiceProvider { get; }

        NodeState State { get; }
    }
}
