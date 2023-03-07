using BenchmarkDotNet.Running;

namespace NI2S.Node.Benchmarks
{
    class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}
