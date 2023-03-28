using Microsoft.Extensions.Hosting;
using System.Threading;

namespace NI2S.Node.Hosting
{
    internal sealed class GenericNodeHostApplicationLifetime : INodeHostLifetime
    {
        private readonly IHostApplicationLifetime _applicationLifetime;
        public GenericNodeHostApplicationLifetime(ApplicationLifetime applicationLifetime)
        {
            _applicationLifetime = applicationLifetime;
        }

        public CancellationToken ApplicationStarted => _applicationLifetime.ApplicationStarted;

        public CancellationToken ApplicationStopping => _applicationLifetime.ApplicationStopping;

        public CancellationToken ApplicationStopped => _applicationLifetime.ApplicationStopped;

        public void StopApplication() => _applicationLifetime.StopApplication();
    }
}
