using System.IO;
using System.Reflection;

namespace NI2S.Node.Infrastructure
{
    internal sealed class AssemblyGlobals
    {
        //public static readonly string ExeName = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
        public static readonly string ExeName = Path.GetFileName(Assembly.GetEntryAssembly().Location);
    }
}
