// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Environment;
using ARWNI2S.Hosting;
using ARWNI2S.Node.Environment;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Node.Hosting.Internal
{
    internal sealed class HostingEnvironment : INiisHostEnvironment
    {
        public string EnvironmentName { get; set; } = Environments.Production;

        public string ApplicationName { get; set; }

        public string NodeRootPath { get; set; } = default!;

        public IFileProvider NodeRootFileProvider { get; set; } = default!;

        public string ContentRootPath { get; set; } = default!;

        public IFileProvider ContentRootFileProvider { get; set; } = default!;

        public IPlatform Platform { get; set; } = NodePlatform.PlatformNull;
    }
}