namespace ARWNI2S.Node
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