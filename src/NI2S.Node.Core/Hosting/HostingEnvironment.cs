using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace NI2S.Node.Hosting
{
    internal sealed class HostingEnvironment : INodeHostEnvironment
    {
        public string EnvironmentName { get; set; } = Environments.Production;

        public string ApplicationName { get; set; }

        public string AssetsRootPath { get; set; } = default!;

        public IFileProvider AssetsRootFileProvider { get; set; } = default!;

        public string ContentRootPath { get; set; } = default!;

        public IFileProvider ContentRootFileProvider { get; set; } = default!;
    }
}
