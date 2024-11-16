using System.Diagnostics;

namespace ARWNI2S.Node.Core.Diagnostics.StackTrace
{
    internal sealed class StackFrameInfo
    {
        public StackFrameInfo(int lineNumber, string filePath, StackFrame stackFrame, MethodDisplayInfo methodDisplayInfo)
        {
            LineNumber = lineNumber;
            FilePath = filePath;
            StackFrame = stackFrame;
            MethodDisplayInfo = methodDisplayInfo;
        }

        public int LineNumber { get; }

        public string FilePath { get; }

        public StackFrame StackFrame { get; }

        public MethodDisplayInfo MethodDisplayInfo { get; }
    }
}