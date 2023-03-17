using System;
using System.Threading.Tasks;

namespace NI2S.Node
{
    public interface INode : INodeContext, IDisposable, IAsyncDisposable
    {
        Task<bool> StartAsync();

        Task StopAsync();
    }
}
