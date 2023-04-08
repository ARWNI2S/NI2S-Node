using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using NI2S.Node.Core.Infrastructure;

namespace NI2S.Node.Hosting
{
#pragma warning disable CS0618 // Type or member is obsolete
    internal sealed class HostingEnvironment : IHostingEnvironment, INodeHostEnvironment
#pragma warning restore CS0618 // Type or member is obsolete
    {
        private IHostEnvironment _hostEnvironment;

        public HostingEnvironment(IHostEnvironment environment, ConfigurationManager configuration)
        {
            _hostEnvironment = environment;

            NodeRootPath = configuration.GetValue<string>(NodeHostDefaults.NodeRootKey);
            NodeRootFileProvider = new NodeFileProvider(this);
        }

        public string NodeRootPath { get; set; }

        public INodeFileProvider NodeRootFileProvider { get; set; }

        public string EnvironmentName { get => _hostEnvironment.EnvironmentName; set => _hostEnvironment.EnvironmentName = value; }

        public string ApplicationName { get => _hostEnvironment.ApplicationName; set => _hostEnvironment.ApplicationName = value; }

        public string ContentRootPath { get => _hostEnvironment.ContentRootPath; set => _hostEnvironment.ContentRootPath = value; }

        public IFileProvider ContentRootFileProvider { get => _hostEnvironment.ContentRootFileProvider; set => _hostEnvironment.ContentRootFileProvider = value; }
    }
}
