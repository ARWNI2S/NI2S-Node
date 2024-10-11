using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Infrastructure.Hosting
{
    public interface IKernelService : IHostedService, IDisposable
    {
    }
}
