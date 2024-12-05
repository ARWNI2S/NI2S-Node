using Microsoft.Extensions.FileProviders;

namespace ARWNI2S.Node.Hosting
{
#pragma warning disable CS0618 // Type or member is obsolete
    internal sealed class HostingEnvironment : IHostingEnvironment, Microsoft.Extensions.Hosting.IHostingEnvironment, INodeHostEnvironment
#pragma warning restore CS0618 // Type or member is obsolete
    {
        public string EnvironmentName { get; set; } = Microsoft.Extensions.Hosting.Environments.Production;

#pragma warning disable CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
        public string ApplicationName { get; set; }
#pragma warning restore CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).

        public string NodeRootPath { get; set; } = default!;

        public IFileProvider NodeRootFileProvider { get; set; } = default!;

        public string ContentRootPath { get; set; } = default!;

        public IFileProvider ContentRootFileProvider { get; set; } = default!;
    }
}