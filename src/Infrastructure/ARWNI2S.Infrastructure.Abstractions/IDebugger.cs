using System.Diagnostics;

namespace ARWNI2S.Infrastructure
{
    internal interface IDebugger
    {
        bool IsAttached { get; }
    }

    internal sealed class DebuggerWrapper : IDebugger
    {
        public static readonly DebuggerWrapper Instance = new();

        private DebuggerWrapper() { }

        public bool IsAttached => Debugger.IsAttached;
    }
}
