using System;
using System.Threading.Tasks;

namespace NI2S.Node
{
    public interface IServer : IServerInfo, IDisposable, IAsyncDisposable
    {
        Task<bool> StartAsync();

        Task StopAsync();
    }
}