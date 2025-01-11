// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics;

namespace ARWNI2S.Diagnostics
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