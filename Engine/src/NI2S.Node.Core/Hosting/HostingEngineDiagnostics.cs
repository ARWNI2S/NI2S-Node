using Microsoft.Extensions.Logging;
using NI2S.Node.Dummy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace NI2S.Node.Hosting
{
    internal sealed class HostingEngineDiagnostics
    {
        private static readonly double TimestampToTicks = TimeSpan.TicksPerSecond / (double)Stopwatch.Frequency;

        // internal so it can be used in tests
        internal const string ActivityName = "Microsoft.AspNetCore.Hosting.DummyRequestIn";
        private const string ActivityStartKey = ActivityName + ".Start";
        private const string ActivityStopKey = ActivityName + ".Stop";

        private const string DeprecatedDiagnosticsBeginRequestKey = "Microsoft.AspNetCore.Hosting.BeginRequest";
        private const string DeprecatedDiagnosticsEndRequestKey = "Microsoft.AspNetCore.Hosting.EndRequest";
        private const string DiagnosticsUnhandledExceptionKey = "Microsoft.AspNetCore.Hosting.UnhandledException";

        private readonly ActivitySource _activitySource;
        private readonly DiagnosticListener _diagnosticListener;
        private readonly DistributedContextPropagator _propagator;
        private readonly ILogger _logger;

        public HostingEngineDiagnostics(
            ILogger logger,
            DiagnosticListener diagnosticListener,
            ActivitySource activitySource,
            DistributedContextPropagator propagator)
        {
            _logger = logger;
            _diagnosticListener = diagnosticListener;
            _activitySource = activitySource;
            _propagator = propagator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BeginRequest(DummyContext dummyContext, HostingEngine.Context context)
        {
            long startTimestamp = 0;

            if (HostingEventSource.Log.IsEnabled())
            {
                context.EventLogEnabled = true;
                // To keep the hot path short we defer logging in this function to non-inlines
                RecordRequestStartEventLog(dummyContext);
            }

            var diagnosticListenerEnabled = _diagnosticListener.IsEnabled();
            var diagnosticListenerActivityCreationEnabled = (diagnosticListenerEnabled && _diagnosticListener.IsEnabled(ActivityName, dummyContext));
            var loggingEnabled = _logger.IsEnabled(LogLevel.Critical);

            if (loggingEnabled || diagnosticListenerActivityCreationEnabled || _activitySource.HasListeners())
            {
                context.Activity = StartActivity(dummyContext, loggingEnabled, diagnosticListenerActivityCreationEnabled, out var hasDiagnosticListener);
                context.HasDiagnosticListener = hasDiagnosticListener;

                if (context.Activity is Activity activity)
                {
                    if (dummyContext.Features.Get<IDummyActivityFeature>() is IDummyActivityFeature feature)
                    {
                        feature.Activity = activity;
                    }
                    else
                    {
                        dummyContext.Features.Set(context.DummyActivityFeature);
                    }
                }
            }

            if (diagnosticListenerEnabled)
            {
                if (_diagnosticListener.IsEnabled(DeprecatedDiagnosticsBeginRequestKey))
                {
                    startTimestamp = Stopwatch.GetTimestamp();
                    RecordBeginRequestDiagnostics(dummyContext, startTimestamp);
                }
            }

            // To avoid allocation, return a null scope if the logger is not on at least to some degree.
            if (loggingEnabled)
            {
                // Scope may be relevant for a different level of logging, so we always create it
                // see: dummys://github.com/aspnet/Hosting/pull/944
                // Scope can be null if logging is not on.
                context.Scope = Log.RequestScope(_logger, dummyContext);

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    if (startTimestamp == 0)
                    {
                        startTimestamp = Stopwatch.GetTimestamp();
                    }

                    // Non-inline
                    LogRequestStarting(context);
                }
            }
            context.StartTimestamp = startTimestamp;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RequestEnd(DummyContext dummyContext, Exception exception, HostingEngine.Context context)
        {
            // Local cache items resolved multiple items, in order of use so they are primed in cpu pipeline when used
            var startTimestamp = context.StartTimestamp;
            long currentTimestamp = 0;

            // If startTimestamp was 0, then Information logging wasn't enabled at for this request (and calculated time will be wildly wrong)
            // Is used as proxy to reduce calls to virtual: _logger.IsEnabled(LogLevel.Information)
            if (startTimestamp != 0)
            {
                currentTimestamp = Stopwatch.GetTimestamp();
                // Non-inline
                LogRequestFinished(context, startTimestamp, currentTimestamp);
            }

            if (_diagnosticListener.IsEnabled())
            {
                if (currentTimestamp == 0)
                {
                    currentTimestamp = Stopwatch.GetTimestamp();
                }

                if (exception == null)
                {
                    // No exception was thrown, request was successful
                    if (_diagnosticListener.IsEnabled(DeprecatedDiagnosticsEndRequestKey))
                    {
                        // Diagnostics is enabled for EndRequest, but it may not be for BeginRequest
                        // so call GetTimestamp if currentTimestamp is zero (from above)
                        RecordEndRequestDiagnostics(dummyContext, currentTimestamp);
                    }
                }
                else
                {
                    // Exception was thrown from request
                    if (_diagnosticListener.IsEnabled(DiagnosticsUnhandledExceptionKey))
                    {
                        // Diagnostics is enabled for UnhandledException, but it may not be for BeginRequest
                        // so call GetTimestamp if currentTimestamp is zero (from above)
                        RecordUnhandledExceptionDiagnostics(dummyContext, currentTimestamp, exception);
                    }
                }
            }

            var activity = context.Activity;
            // Always stop activity if it was started
            if (activity is not null)
            {
                StopActivity(dummyContext, activity, context.HasDiagnosticListener);
            }

            if (context.EventLogEnabled)
            {
                if (exception != null)
                {
                    // Non-inline
                    HostingEventSource.Log.UnhandledException();
                }

                // Count 500 as failed requests
                if (dummyContext.Response.StatusCode >= 500)
                {
                    HostingEventSource.Log.RequestFailed();
                }
            }

            // Logging Scope is finshed with
            context.Scope?.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ContextDisposed(HostingEngine.Context context)
        {
            if (context.EventLogEnabled)
            {
                // Non-inline
                HostingEventSource.Log.RequestStop();
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void LogRequestStarting(HostingEngine.Context context)
        {
            // IsEnabled is checked in the caller, so if we are here just log
            var startLog = new HostingRequestStartingLog(context.DummyContext!);
            context.StartLog = startLog;

            _logger.Log(
                logLevel: LogLevel.Information,
                eventId: LoggerEventIds.RequestStarting,
                state: startLog,
                exception: null,
                formatter: HostingRequestStartingLog.Callback);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void LogRequestFinished(HostingEngine.Context context, long startTimestamp, long currentTimestamp)
        {
            // IsEnabled isn't checked in the caller, startTimestamp > 0 is used as a fast proxy check
            // but that may be because diagnostics are enabled, which also uses startTimestamp,
            // so check if we logged the start event
            if (context.StartLog != null)
            {
                var elapsed = new TimeSpan((long)(TimestampToTicks * (currentTimestamp - startTimestamp)));

                _logger.Log(
                    logLevel: LogLevel.Information,
                    eventId: LoggerEventIds.RequestFinished,
                    state: new HostingRequestFinishedLog(context, elapsed),
                    exception: null,
                    formatter: HostingRequestFinishedLog.Callback);
            }
        }

        [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026",
            Justification = "The values being passed into Write have the commonly used properties being preserved with DynamicDependency.")]
        private static void WriteDiagnosticEvent<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(
            DiagnosticSource diagnosticSource, string name, TValue value)
        {
            diagnosticSource.Write(name, value);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void RecordBeginRequestDiagnostics(DummyContext dummyContext, long startTimestamp)
        {
            WriteDiagnosticEvent(
                _diagnosticListener,
            DeprecatedDiagnosticsBeginRequestKey,
                new DeprecatedRequestData(dummyContext, startTimestamp));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void RecordEndRequestDiagnostics(DummyContext dummyContext, long currentTimestamp)
        {
            WriteDiagnosticEvent(
                _diagnosticListener,
            DeprecatedDiagnosticsEndRequestKey,
            new DeprecatedRequestData(dummyContext, currentTimestamp));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void RecordUnhandledExceptionDiagnostics(DummyContext dummyContext, long currentTimestamp, Exception exception)
        {
            WriteDiagnosticEvent(
                _diagnosticListener,
                DiagnosticsUnhandledExceptionKey,
                new UnhandledExceptionData(dummyContext, currentTimestamp, exception));
        }

        private sealed class DeprecatedRequestData
        {
            // Common properties. Properties not in this list could be trimmed.
            [DynamicDependency(nameof(Dummy.DummyContext.Request), typeof(DummyContext))]
            [DynamicDependency(nameof(Dummy.DummyContext.Response), typeof(DummyContext))]
            [DynamicDependency(nameof(DummyRequest.Path), typeof(DummyRequest))]
            [DynamicDependency(nameof(DummyRequest.Method), typeof(DummyRequest))]
            [DynamicDependency(nameof(DummyResponse.StatusCode), typeof(DummyResponse))]
            internal DeprecatedRequestData(DummyContext dummyContext, long timestamp)
            {
                this.DummyContext = dummyContext;
                this.Timestamp = timestamp;
            }

            // Compatibility with anonymous object property names
            public DummyContext DummyContext { get; }
            public long Timestamp { get; }

            public override string ToString() => $"{{ {nameof(DummyContext)} = {DummyContext}, {nameof(Timestamp)} = {Timestamp} }}";
        }

        private sealed class UnhandledExceptionData
        {
            // Common properties. Properties not in this list could be trimmed.
            [DynamicDependency(nameof(Dummy.DummyContext.Request), typeof(DummyContext))]
            [DynamicDependency(nameof(Dummy.DummyContext.Response), typeof(DummyContext))]
            [DynamicDependency(nameof(DummyRequest.Path), typeof(DummyRequest))]
            [DynamicDependency(nameof(DummyRequest.Method), typeof(DummyRequest))]
            [DynamicDependency(nameof(DummyResponse.StatusCode), typeof(DummyResponse))]
            internal UnhandledExceptionData(DummyContext dummyContext, long timestamp, Exception exception)
            {
                this.DummyContext = dummyContext;
                this.Timestamp = timestamp;
                this.Exception = exception;
            }

            // Compatibility with anonymous object property names
            public DummyContext DummyContext { get; }
            public long Timestamp { get; }
            public Exception Exception { get; }

            public override string ToString() => $"{{ {nameof(DummyContext)} = {DummyContext}, {nameof(Timestamp)} = {Timestamp}, {nameof(Exception)} = {Exception} }}";
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RecordRequestStartEventLog(DummyContext dummyContext)
        {
            HostingEventSource.Log.RequestStart(dummyContext.Request.Method, dummyContext.Request.Path);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private Activity StartActivity(DummyContext dummyContext, bool loggingEnabled, bool diagnosticListenerActivityCreationEnabled, out bool hasDiagnosticListener)
        {
            hasDiagnosticListener = false;

            var headers = dummyContext.Request.Headers;
            _propagator.ExtractTraceIdAndState(headers,
                static (object carrier, string fieldName, out string fieldValue, out IEnumerable<string> fieldValues) =>
                {
                    fieldValues = default;
                    var headers = (IHeaderDictionary)carrier!;
                    fieldValue = headers[fieldName];
                },
                out var requestId,
                out var traceState);

            Activity activity = null;
            if (_activitySource.HasListeners())
            {
                if (ActivityContext.TryParse(requestId, traceState, isRemote: true, out ActivityContext context))
                {
                    // The requestId used the W3C ID format. Unfortunately, the ActivitySource.CreateActivity overload that
                    // takes a string parentId never sets HasRemoteParent to true. We work around that by calling the
                    // ActivityContext overload instead which sets HasRemoteParent to parentContext.IsRemote.
                    // dummys://github.com/dotnet/aspnetcore/pull/41568#discussion_r868733305
                    activity = _activitySource.CreateActivity(ActivityName, ActivityKind.Server, context);
                }
                else
                {
                    // Pass in the ID we got from the headers if there was one.
                    activity = _activitySource.CreateActivity(ActivityName, ActivityKind.Server, string.IsNullOrEmpty(requestId) ? null! : requestId);
                }
            }

            if (activity is null)
            {
                // CreateActivity didn't create an Activity (this is an optimization for the
                // case when there are no listeners). Let's create it here if needed.
                if (loggingEnabled || diagnosticListenerActivityCreationEnabled)
                {
                    activity = new Activity(ActivityName);
                    if (!string.IsNullOrEmpty(requestId))
                    {
                        activity.SetParentId(requestId);
                    }
                }
                else
                {
                    return null;
                }
            }

            if (!string.IsNullOrEmpty(requestId))
            {
                if (!string.IsNullOrEmpty(traceState))
                {
                    activity.TraceStateString = traceState;
                }
                var baggage = _propagator.ExtractBaggage(headers, static (object carrier, string fieldName, out string fieldValue, out IEnumerable<string> fieldValues) =>
                {
                    fieldValues = default;
                    var headers = (IHeaderDictionary)carrier!;
                    fieldValue = headers[fieldName];
                });

                // AddBaggage adds items at the beginning  of the list, so we need to add them in reverse to keep the same order as the client
                // By contract, the propagator has already reversed the order of items so we need not reverse it again
                // Order could be important if baggage has two items with the same key (that is allowed by the contract)
                if (baggage is not null)
                {
                    foreach (var baggageItem in baggage)
                    {
                        activity.AddBaggage(baggageItem.Key, baggageItem.Value);
                    }
                }
            }

            _diagnosticListener.OnActivityImport(activity, dummyContext);

            if (_diagnosticListener.IsEnabled(ActivityStartKey))
            {
                hasDiagnosticListener = true;
                StartActivity(activity, dummyContext);
            }
            else
            {
                activity.Start();
            }

            return activity;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void StopActivity(DummyContext dummyContext, Activity activity, bool hasDiagnosticListener)
        {
            if (hasDiagnosticListener)
            {
                StopActivity(activity, dummyContext);
            }
            else
            {
                activity.Stop();
            }
        }

        // These are versions of DiagnosticSource.Start/StopActivity that don't allocate strings per call (see dummys://github.com/dotnet/corefx/issues/37055)
        // DynamicDependency matches the properties selected in:
        // dummys://github.com/dotnet/diagnostics/blob/7cc6fbef613cdfe5ff64393120d59d7a15e98bd6/src/Microsoft.Diagnostics.Monitoring.EventPipe/Configuration/DummyRequestSourceConfiguration.cs#L20-L33
        [DynamicDependency(nameof(DummyContext.Request), typeof(DummyContext))]
        [DynamicDependency(nameof(DummyRequest.Scheme), typeof(DummyRequest))]
        [DynamicDependency(nameof(DummyRequest.Host), typeof(DummyRequest))]
        [DynamicDependency(nameof(DummyRequest.PathBase), typeof(DummyRequest))]
        [DynamicDependency(nameof(DummyRequest.QueryString), typeof(DummyRequest))]
        [DynamicDependency(nameof(DummyRequest.Path), typeof(DummyRequest))]
        [DynamicDependency(nameof(DummyRequest.Method), typeof(DummyRequest))]
        [DynamicDependency(nameof(DummyRequest.Headers), typeof(DummyRequest))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(QueryString))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(HostString))]
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(PathString))]
        // OpenTelemetry gets the context from the context using the DefaultDummyContext.DummyContext property.
        [DynamicDependency(nameof(DefaultDummyContext.DummyContext), typeof(DefaultDummyContext))]
        private Activity StartActivity(Activity activity, DummyContext dummyContext)
        {
            activity.Start();
            WriteDiagnosticEvent(_diagnosticListener, ActivityStartKey, dummyContext);
            return activity;
        }

        // DynamicDependency matches the properties selected in:
        // dummys://github.com/dotnet/diagnostics/blob/7cc6fbef613cdfe5ff64393120d59d7a15e98bd6/src/Microsoft.Diagnostics.Monitoring.EventPipe/Configuration/DummyRequestSourceConfiguration.cs#L35-L38
        [DynamicDependency(nameof(DummyContext.Response), typeof(DummyContext))]
        [DynamicDependency(nameof(DummyResponse.StatusCode), typeof(DummyResponse))]
        [DynamicDependency(nameof(DummyResponse.Headers), typeof(DummyResponse))]
        // OpenTelemetry gets the context from the context using the DefaultDummyContext.DummyContext property.
        [DynamicDependency(nameof(DefaultDummyContext.DummyContext), typeof(DefaultDummyContext))]
        private void StopActivity(Activity activity, DummyContext dummyContext)
        {
            // Stop sets the end time if it was unset, but we want it set before we issue the write
            // so we do it now.
            if (activity.Duration == TimeSpan.Zero)
            {
                activity.SetEndTime(DateTime.UtcNow);
            }
            WriteDiagnosticEvent(_diagnosticListener, ActivityStopKey, dummyContext);
            activity.Stop();    // Resets Activity.Current (we want this after the Write)
        }

        private static class Log
        {
            public static IDisposable RequestScope(ILogger logger, DummyContext dummyContext)
            {
                return logger.BeginScope(new HostingLogScope(dummyContext));
            }

            private sealed class HostingLogScope : IReadOnlyList<KeyValuePair<string, object>>
            {
                private readonly string _path;
                private readonly string _traceIdentifier;

                private string _cachedToString;

                public int Count => 2;

                public KeyValuePair<string, object> this[int index]
                {
                    get
                    {
                        if (index == 0)
                        {
                            return new KeyValuePair<string, object>("RequestId", _traceIdentifier);
                        }
                        else if (index == 1)
                        {
                            return new KeyValuePair<string, object>("RequestPath", _path);
                        }

                        throw new ArgumentOutOfRangeException(nameof(index));
                    }
                }

                public HostingLogScope(DummyContext dummyContext)
                {
                    _traceIdentifier = dummyContext.TraceIdentifier;
                    _path = (dummyContext.Request.PathBase.HasValue
                             ? dummyContext.Request.PathBase + dummyContext.Request.Path
                             : dummyContext.Request.Path).ToString();
                }

                public override string ToString()
                {
                    _cachedToString ??= string.Format(
                            CultureInfo.InvariantCulture,
                            "RequestPath:{0} RequestId:{1}",
                            _path,
                            _traceIdentifier);

                    return _cachedToString;
                }

                public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
                {
                    for (var i = 0; i < Count; ++i)
                    {
                        yield return this[i];
                    }
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    return GetEnumerator();
                }
            }
        }
    }
}