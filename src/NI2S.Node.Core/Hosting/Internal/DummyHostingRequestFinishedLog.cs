using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace NI2S.Node.Hosting.Internal
{
    internal sealed class DummyHostingRequestFinishedLog : IReadOnlyList<KeyValuePair<string, object>>
    {
        internal static readonly Func<object, Exception, string> Callback = (state, exception) => ((DummyHostingRequestFinishedLog)state).ToString();

        private const string OriginalFormat = "Request finished {Protocol} {Method} {Scheme}://{Host}{PathBase}{Path}{QueryString} - {StatusCode} {ContentLength} {ContentType} {ElapsedMilliseconds}ms";
        private readonly DummyHostingApplication.Context _context;

        private string _cachedToString;
        public TimeSpan Elapsed { get; }

        public int Count => 12;

        public KeyValuePair<string, object> this[int index]
        {
            get
            {
                Debug.Assert(_context.DummyContext != null);

                var request = _context.DummyContext.Request;
                var response = _context.DummyContext.Response;

                return index switch
                {
                    0 => new KeyValuePair<string, object>("ElapsedMilliseconds", Elapsed.TotalMilliseconds),
                    1 => new KeyValuePair<string, object>(nameof(response.StatusCode), response.StatusCode),
                    2 => new KeyValuePair<string, object>(nameof(response.ContentType), response.ContentType),
                    3 => new KeyValuePair<string, object>(nameof(response.ContentLength), response.ContentLength),
                    4 => new KeyValuePair<string, object>(nameof(request.Protocol), request.Protocol),
                    5 => new KeyValuePair<string, object>(nameof(request.Method), request.Method),
                    6 => new KeyValuePair<string, object>(nameof(request.Scheme), request.Scheme),
                    7 => new KeyValuePair<string, object>(nameof(request.Host), request.Host),
                    8 => new KeyValuePair<string, object>(nameof(request.PathBase), request.PathBase),
                    9 => new KeyValuePair<string, object>(nameof(request.Path), request.Path),
                    10 => new KeyValuePair<string, object>(nameof(request.QueryString), request.QueryString),
                    11 => new KeyValuePair<string, object>("{OriginalFormat}", OriginalFormat),
                    _ => throw new IndexOutOfRangeException(nameof(index)),
                };
            }
        }

        public DummyHostingRequestFinishedLog(DummyHostingApplication.Context context, TimeSpan elapsed)
        {
            _context = context;
            Elapsed = elapsed;
        }

        public override string ToString()
        {
            if (_cachedToString == null)
            {
                Debug.Assert(_context.DummyContext != null);

                var request = _context.DummyContext.Request;
                var response = _context.DummyContext.Response;
                //_cachedToString = $"Request finished {request.Protocol} {request.Method} {request.Scheme}://{request.Host}{request.PathBase}{request.Path}{request.QueryString} - {response.StatusCode.ToString(CultureInfo.InvariantCulture)} {ValueOrEmptyMarker(response.ContentLength)} {EscapedValueOrEmptyMarker(response.ContentType)} {Elapsed.TotalMilliseconds.ToString("0.0000", CultureInfo.InvariantCulture)}ms";
            }

            return _cachedToString;
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
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