using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Node.Hosting.Internal
{
#pragma warning disable CS0618 // Type or member is obsolete
    internal sealed class HostingEnvironment : INiisHostEnvironment, IHostingEnvironment
#pragma warning restore CS0618 // Type or member is obsolete
    {
        public string EnvironmentName { get; set; } = Environments.Production;

        public string ApplicationName { get; set; }

        public string NI2SRootPath { get; set; } = default!;

        public IFileProvider NI2SRootFileProvider { get; set; } = default!;

        public string ContentRootPath { get; set; } = default!;

        public IFileProvider ContentRootFileProvider { get; set; } = default!;
    }
}