// TODO: find and replace this line with COPYRIGTH NOTICE entire solution

using Microsoft.Extensions.Hosting;

namespace TestNode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var builder = NodeHost.CreateHostBuilder(args);
            var builder2 = Host.CreateApplicationBuilder(args);
            //builder.UseOrleans(siloBuilder => siloBuilder.UseLocalhostClustering())
            //var app = builder.Build();
            var app2 = builder2.Build();
        }
    }
}