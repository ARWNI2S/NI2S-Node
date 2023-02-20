using System.Runtime.InteropServices;
using System.Security;

namespace NI2S.Node.Interop
{
    public static partial class NativeMethods
    {
        [DllImport("Dbghelp.dll")]
        [SuppressUnmanagedCodeSecurity()]
        public static extern bool MiniDumpWriteDump(
            IntPtr hProcess,
            int processId,
            IntPtr hFile,
            MiniDumpType dumpType,
            IntPtr exceptionParam,
            IntPtr userStreamParam,
            IntPtr callbackParam);
    }
}
