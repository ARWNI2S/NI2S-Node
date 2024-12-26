using ARWNI2S.Node;
using System.Reflection;

namespace EmptyNode
{
    public class Program
    {
        [MTAThread]
        public static void Main(string[] args)
        {
            Console.WriteLine($"{nameof(Program)}.{MethodBase.GetCurrentMethod()?.Name} - Thread - {Thread.CurrentThread.ManagedThreadId}");
            NI2SNode.Create(args).Run();
            Console.WriteLine($"{nameof(Program)}.{MethodBase.GetCurrentMethod()?.Name} - Thread - {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}