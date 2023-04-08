using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace NI2S.Node.Hosting
{
    public interface INodeEngine : IHost
    {
        void Run();

        Task RunAsync();
    }
}
