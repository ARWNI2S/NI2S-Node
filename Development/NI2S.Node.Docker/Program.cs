using NI2S.Node.Hosting;

namespace NI2S.Node.Docker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            INodeHost host = NI2SNode.CreateDefaultBuilder(args)
                //.ConfigureServices(services =>
                //{
                //    services.AddHostedService<Worker>();
                //})
                .Build();

            host.Run();
        }
    }
}