using NI2S.Node.Diagnostics.Debugger;
using System.Collections.Generic;

namespace NI2S.Node.Diagnostics
{
    public enum DebugCode : uint
    {
        None = 0,
        Critical = 99999999
    }

    internal static class DebugCodes
    {
        private static readonly Dictionary<uint, IDebugCode> debugCodes = new Dictionary<uint, IDebugCode>()
        {
            { 0, new ErrorCode("00000000","None",string.Empty) },
            { 99999999, new ErrorCode("99999999", "Critical", "Unexpected unknown critical error code.") },
        };

        public static IDebugCode GetInfo(this DebugCode debugCode)
        {
            return debugCodes[(uint)debugCode];
        }
    }
}
