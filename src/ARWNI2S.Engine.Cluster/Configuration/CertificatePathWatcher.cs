// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

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