using NI2S.Node.Globalization.Resources;
using System;

namespace NI2S.Node.Infrastructure
{
    public enum OperationResult
    {
        Failure = 0,
        Success,
        Warnings,
        Errors,
    }

    public struct OperationInfo
    {
        internal static readonly OperationInfo Success = SuccessWithState();

        internal static OperationInfo SuccessWithState(object state = null)
        {
            return new OperationInfo
            {
                Message = LocalizedStrings.OperationResult_Success_Message,
                State = state,
                Result = state != null ? OperationResult.Warnings : OperationResult.Success
            };
        }
        internal static OperationInfo SuccessWithState(Exception exception)
        {
            return new OperationInfo
            {
                Message = exception.Message,
                State = exception,
                Result = OperationResult.Warnings
            };
        }

        internal static readonly OperationInfo Failure = FailWithException(null);

        internal static OperationInfo FailWithException(Exception ex)
        {
            return new OperationInfo
            {
                Message = LocalizedStrings.OperationResult_Failure_Message,
                State = ex,
                Result = ex != null ? OperationResult.Errors : OperationResult.Failure
            };
        }

        internal static OperationInfo FailWithException(string message, Exception ex)
        {
            return new OperationInfo
            {
                Message = message,
                State = ex,
                Result = OperationResult.Errors
            };
        }

        public OperationResult Result { get; private set; }
        public string Message { get; private set; }
        public object State { get; private set; }
    }
}
