using ARWNI2S.Node;

namespace EmptyNode.Docker
{
    public class Program
    {
        [MTAThread]
        public static void Main(string[] args)
        {
            NI2SNode.Create(args).Run();
        }
    }
}