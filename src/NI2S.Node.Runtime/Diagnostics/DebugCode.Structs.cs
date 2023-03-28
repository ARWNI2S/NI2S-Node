using NI2S.Node.Diagnostics.Debugger;
using System;

namespace NI2S.Node.Diagnostics
{
    [Serializable]
    internal readonly struct ErrorCode : IDebugCode
    {
        public readonly DebugLevel Level = DebugLevel.Error;
        public readonly string Code { get; }
        public readonly string Error { get; }
        public readonly string Messsage { get; }

        public ErrorCode(string code, string error, string messsage)
        {
            Code = code;
            Error = error;
            Messsage = messsage;
        }

        DebugLevel IDebugCode.Level => Level;
        string IDebugCode.Id => Code;
        string IDebugCode.Name => Error;
    }

    [Serializable]
    internal readonly struct WarningCode : IDebugCode
    {
        public readonly DebugLevel Level = DebugLevel.Warning;
        public readonly string Code { get; }
        public readonly string Warning { get; }
        public readonly string Messsage { get; }

        public WarningCode(string code, string warning, string messsage)
        {
            Code = code;
            Warning = warning;
            Messsage = messsage;
        }

        DebugLevel IDebugCode.Level => Level;
        string IDebugCode.Id => Code;
        string IDebugCode.Name => Warning;
    }

    [Serializable]
    internal readonly struct RuntimeCode : IDebugCode
    {
        public readonly DebugLevel Level = DebugLevel.Info;
        public readonly string Code { get; }
        public readonly string Name { get; }
        public readonly string Messsage { get; }

        public RuntimeCode(string code, string name, string messsage)
        {
            Code = code;
            Name = name;
            Messsage = messsage;
        }

        DebugLevel IDebugCode.Level => Level;
        string IDebugCode.Id => Code;
    }
}
