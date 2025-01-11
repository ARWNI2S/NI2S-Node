using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ARWNI2S.Cluster.Configuration
{
    internal class CertificatePathWatcher
    {
        private IHostEnvironment _hostEnvironment;
        private ILogger _logger;

        public CertificatePathWatcher(IHostEnvironment hostEnvironment, ILogger logger)
        {
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }
    }
}