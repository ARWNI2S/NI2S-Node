using NI2S.Node.Dummy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace NI2S.Node.Hosting
{
    internal sealed class HostingRequestStartingLog : IReadOnlyList<KeyValuePair<string, object>>
    {
        private const string LogPreamble = "Request starting ";
        private const string EmptyEntry = "-";

        internal static readonly Func<object, Exception, string> Callback = (state, exception) => ((HostingRequestStartingLog)state).ToString();

        private readonly DummyRequest _request;

        private string _cachedToString;

        public int Count => 9;

        public KeyValuePair<string, object> this[int index] => index switch
        {
            0 => new KeyValuePair<string, object>(nameof(_request.Protocol), _request.Protocol),
            1 => new KeyValuePair<string, object>(nameof(_request.Method), _request.Method),
            2 => new KeyValuePair<string, object>(nameof(_request.ContentType), _request.ContentType),
            3 => new KeyValuePair<string, object>(nameof(_request.ContentLength), _request.ContentLength),
            4 => new KeyValuePair<string, object>(nameof(_request.Scheme), _request.Scheme),
            5 => new KeyValuePair<string, object>(nameof(_request.Host), _request.Host),
            6 => new KeyValuePair<string, object>(nameof(_request.PathBase), _request.PathBase),
            7 => new KeyValuePair<string, object>(nameof(_request.Path), _request.Path),
            8 => new KeyValuePair<string, object>(nameof(_request.QueryString), _request.QueryString),
            _ => throw new IndexOutOfRangeException(nameof(index)),
        };

        public HostingRequestStartingLog(DummyContext dummyContext)
        {
            _request = dummyContext.Request;
        }

        public override string ToString()
        {
            if (_cachedToString == null)
            {
                var request = _request;
                //_cachedToString = $"{LogPreamble}{request.Protocol} {request.Method} {request.Scheme}://{request.Host}{request.PathBase}{request.Path}{request.QueryString} {EscapedValueOrEmptyMarker(request.ContentType)} {ValueOrEmptyMarker<int>(request.ContentLength)}"; ;
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

        internal string ToStringWithoutPreamble()
            => ToString()[LogPreamble.Length..];

        internal static string EscapedValueOrEmptyMarker(string potentialValue)
            // Encode space as +
            => potentialValue?.Length > 0 ? potentialValue.Replace(' ', '+') : EmptyEntry;

        internal static string ValueOrEmptyMarker<T>(T? potentialValue) where T : struct, IFormattable
            => potentialValue?.ToString(null, CultureInfo.InvariantCulture) ?? EmptyEntry;
    }
}