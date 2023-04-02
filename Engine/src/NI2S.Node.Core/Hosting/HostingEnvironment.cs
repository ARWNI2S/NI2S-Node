using Microsoft.Extensions.FileProviders;

namespace NI2S.Node.Hosting
{
#pragma warning disable CS0618 // Type or member is obsolete
    internal sealed class HostingEnvironment : IHostingEnvironment, Microsoft.Extensions.Hosting.IHostingEnvironment, INodeHostEnvironment
#pragma warning restore CS0618 // Type or member is obsolete
    {
        public string EnvironmentName { get; set; } = Microsoft.Extensions.Hosting.Environments.Production;

        public string ApplicationName { get; set; }

        public string NodeRootPath { get; set; } = default!;

        public IFileProvider NodeRootFileProvider { get; set; } = default!;

        public string ContentRootPath { get; set; } = default!;

        public IFileProvider ContentRootFileProvider { get; set; } = default!;
    }
}
