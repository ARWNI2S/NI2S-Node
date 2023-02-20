using System.Runtime.InteropServices;
using System.Security;

namespace NI2S.Node.Interop
{
    /// <summary>
    /// Encapsula las funciones externas, dependientes de Win32.
    /// </summary>
    public static partial class NativeMethods
    {
        /// <summary>
        /// Query processor performance frequency.
        /// </summary>
        /// <param name="lpFrequency">referenced performance frequency as long.</param>
        /// <returns>true if operation was successful.</returns>
        [DllImport("kernel32")]
        [SuppressUnmanagedCodeSecurity()]
        public static extern bool QueryPerformanceFrequency(out long lpFrequency);
        /// <summary>
        /// Query processor performance frequency.
        /// </summary>
        /// <param name="lpFrequency">referenced performance frequency as ulong.</param>
        /// <returns>true if operation was successful.</returns>
        [DllImport("kernel32")]
        [SuppressUnmanagedCodeSecurity()]
        public static extern bool QueryPerformanceFrequency(out ulong lpFrequency);

        /// <summary>
        /// Query processor performance counter.
        /// </summary>
        /// <param name="lpCounter">referenced performance counter as long</param>
        /// <returns>true if operation was successful.</returns>
        [DllImport("kernel32")]
        [SuppressUnmanagedCodeSecurity()]
        public static extern bool QueryPerformanceCounter(out long lpCounter);
        /// <summary>
        /// Query processor performance counter.
        /// </summary>
        /// <param name="lpCounter">referenced performance counter as ulong</param>
        /// <returns>true if operation was successful.</returns>
        [DllImport("kernel32")]
        [SuppressUnmanagedCodeSecurity()]
        public static extern bool QueryPerformanceCounter(out ulong lpCounter);

        /// <summary>
        /// Retrieves the cycle time for the specified thread.
        /// </summary>
        /// <param name="threadHandle">A handle to the thread. The handle must have the PROCESS_QUERY_INFORMATION or PROCESS_QUERY_LIMITED_INFORMATION access right.</param>
        /// <param name="cycleTime">The number of CPU clock cycles used by the thread. This value includes cycles spent in both user mode and kernel mode.</param>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        [SuppressUnmanagedCodeSecurity()]
        public static extern bool QueryThreadCycleTime(IntPtr threadHandle, out ulong cycleTime);

        /// <summary>
        /// Retrieves a pseudo handle for the calling thread.
        /// </summary>
        /// <returns>A pseudo handle for the current thread.</returns>
        [DllImport("Kernel32.dll")]
        [SuppressUnmanagedCodeSecurity()]
        public static extern IntPtr GetCurrentThread();
    }
}
