namespace ARWNI2S.Node.EmptyTest
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await NI2SNode.Create(args).RunAsync();
        }
    }
}