// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Cluster.Environment;
using Microsoft.Extensions.Logging;

namespace ARWNI2S.Engine.Cluster.Logging
{
    internal sealed partial class ClusterTrace : ILogger
    {
        public void ConnectionBadRequest(string connectionId, BadRequestException ex)
        {
            BadRequestsLog.ConnectionBadRequest(_badRequestsLogger, connectionId, ex.Message, ex);
        }

        public void RequestProcessingError(string connectionId, Exception ex)
        {
            BadRequestsLog.RequestProcessingError(_badRequestsLogger, connectionId, ex);
        }

        public void RequestBodyMinimumDataRateNotSatisfied(string connectionId, string traceIdentifier, double rate)
        {
            BadRequestsLog.RequestBodyMinimumDataRateNotSatisfied(_badRequestsLogger, connectionId, traceIdentifier, rate);
        }

        public void ResponseMinimumDataRateNotSatisfied(string connectionId, string traceIdentifier)
        {
            BadRequestsLog.ResponseMinimumDataRateNotSatisfied(_badRequestsLogger, connectionId, traceIdentifier);
        }

        //public void PossibleInvalidHttpVersionDetected(string connectionId, HttpVersion expectedHttpVersion, HttpVersion detectedHttpVersion)
        //{
        //    if (_generalLogger.IsEnabled(LogLevel.Debug))
        //    {
        //        BadRequestsLog.PossibleInvalidHttpVersionDetected(_badRequestsLogger, connectionId, HttpUtilities.VersionToString(expectedHttpVersion), HttpUtilities.VersionToString(detectedHttpVersion));
        //    }
        //}

        private static partial class BadRequestsLog
        {
            [LoggerMessage(17, LogLevel.Debug, @"Connection id ""{ConnectionId}"" bad request data: ""{message}""", EventName = "ConnectionBadRequest")]
            public static partial void ConnectionBadRequest(ILogger logger, string connectionId, string message, BadRequestException ex);

            [LoggerMessage(20, LogLevel.Debug, @"Connection id ""{ConnectionId}"" request processing ended abnormally.", EventName = "RequestProcessingError")]
            public static partial void RequestProcessingError(ILogger logger, string connectionId, Exception ex);

            [LoggerMessage(27, LogLevel.Debug, @"Connection id ""{ConnectionId}"", Request id ""{TraceIdentifier}"": the request timed out because it was not sent by the client at a minimum of {Rate} bytes/second.", EventName = "RequestBodyMinimumDataRateNotSatisfied")]
            public static partial void RequestBodyMinimumDataRateNotSatisfied(ILogger logger, string connectionId, string traceIdentifier, double rate);

            [LoggerMessage(28, LogLevel.Debug, @"Connection id ""{ConnectionId}"", Request id ""{TraceIdentifier}"": the connection was closed because the response was not read by the client at the specified minimum data rate.", EventName = "ResponseMinimumDataRateNotSatisfied")]
            public static partial void ResponseMinimumDataRateNotSatisfied(ILogger logger, string connectionId, string traceIdentifier);

            [LoggerMessage(54, LogLevel.Debug, @"Connection id ""{ConnectionId}"": Invalid content received on connection. Possible incorrect HTTP version detected. Expected {ExpectedHttpVersion} but received {DetectedHttpVersion}.", EventName = "PossibleInvalidHttpVersionDetected", SkipEnabledCheck = true)]
            public static partial void PossibleInvalidHttpVersionDetected(ILogger logger, string connectionId, string expectedHttpVersion, string detectedHttpVersion);

            // IDs prior to 64 are reserved for back compat (the various ClusterTrace loggers used to share a single sequence)
        }
    }
}
