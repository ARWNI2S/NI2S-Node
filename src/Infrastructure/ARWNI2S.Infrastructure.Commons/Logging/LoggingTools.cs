using ARWNI2S.Infrastructure.Resources;
using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace ARWNI2S.Infrastructure.Logging
{
    internal static class LoggingTools
    {
        private static readonly ConcurrentDictionary<Type, Func<Exception, string>> exceptionDecoders = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptionType"></param>
        /// <param name="decoder"></param>
        public static void SetExceptionDecoder(Type exceptionType, Func<Exception, string> decoder)
        {
            exceptionDecoders.TryAdd(exceptionType, decoder);
        }

        // http://www.csharp-examples.net/string-format-DateTime/
        // http://msdn.microsoft.com/en-us/library/system.globalization.DateTimeformatinfo.aspx
        private const string TIME_FORMAT = "HH:mm:ss.fff 'GMT'"; // Example: 09:50:43.341 GMT
        private const string DATE_FORMAT = "yyyy-MM-dd " + TIME_FORMAT; // Example: 2010-09-02 09:50:43.341 GMT - Variant of UniversalSorta­bleDateTimePat­tern

        /// <summary>
        /// Utility function to convert a <c>DateTime</c> object into printable data format used by the TraceLogger subsystem.
        /// </summary>
        /// <param name="date">The <c>DateTime</c> value to be printed.</param>
        /// <returns>Formatted string representation of the input data, in the printable format used by the TraceLogger subsystem.</returns>
        public static string PrintDate(this DateTime date)
        {
            return date.ToString(DATE_FORMAT, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Utility function to convert a <c>string</c> object into data format used by the system.
        /// </summary>
        /// <param name="dateStr"></param>
        /// <returns></returns>
        public static DateTime ParseDate(this string dateStr)
        {
            return DateTime.ParseExact(dateStr, DATE_FORMAT, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Utility function to convert a <c>DateTime</c> object into printable time format used by the TraceLogger subsystem.
        /// </summary>
        /// <param name="date">The <c>DateTime</c> value to be printed.</param>
        /// <returns>Formatted string representation of the input data, in the printable format used by the TraceLogger subsystem.</returns>
        public static string PrintTime(this DateTime date)
        {
            return date.ToString(TIME_FORMAT, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Utility function to convert an exception into printable format, including expanding and formatting any nested sub-expressions.
        /// </summary>
        /// <param name="exception">The exception to be printed.</param>
        /// <returns>Formatted string representation of the exception, including expanding and formatting any nested sub-expressions.</returns>
        public static string PrintException(this Exception exception)
        {
            return exception == null ? string.Empty : PrintException_Helper(exception, 0, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string PrintExceptionWithoutStackTrace(this Exception exception)
        {
            return exception == null ? string.Empty : PrintException_Helper(exception, 0, false);
        }


        #region Utils

        private static string FormatMessageText(string format, object[] args)
        {
            // avoids exceptions if format string contains braces in calls that were not
            // designed to use format strings
            return args == null || args.Length == 0 ? format : string.Format(format, args);
        }

        private static string PrintException_Helper(Exception exception, int level, bool includeStackTrace)
        {
            if (exception == null) return string.Empty;
            var sb = new StringBuilder();
            sb.Append(PrintOneException(exception, level, includeStackTrace));
            if (exception is ReflectionTypeLoadException refTypLoEx)
            {
                Exception[] loaderExceptions =
                    refTypLoEx.LoaderExceptions;
                if (loaderExceptions == null || loaderExceptions.Length == 0)
                {
                    sb.Append(LocalizedStrings.TraceLogger_PrintException_Helper_NoExceptionsMesage);
                }
                else
                {
                    foreach (Exception inner in loaderExceptions)
                    {
                        // call recursively on all loader exceptions. Same level for all.
                        sb.Append(PrintException_Helper(inner, level + 1, includeStackTrace));
                    }
                }
            }
            else if (exception is AggregateException aggEx)
            {
                var innerExceptions = aggEx.InnerExceptions;
                if (innerExceptions == null) return sb.ToString();

                foreach (Exception inner in innerExceptions)
                {
                    // call recursively on all inner exceptions. Same level for all.
                    sb.Append(PrintException_Helper(inner, level + 1, includeStackTrace));
                }
            }
            else if (exception.InnerException != null)
            {
                // call recursively on a single inner exception.
                sb.Append(PrintException_Helper(exception.InnerException, level + 1, includeStackTrace));
            }
            return sb.ToString();
        }

        private static string PrintOneException(this Exception exception, int level, bool includeStackTrace)
        {
            if (exception == null) return string.Empty;
            string stack = string.Empty;
            if (includeStackTrace && exception.StackTrace != null)
                stack = string.Format(Environment.NewLine + exception.StackTrace);

            string message = exception.Message;
            var excType = exception.GetType();

            if (exceptionDecoders.TryGetValue(excType, out Func<Exception, string> decoder))
                message = decoder(exception);

            return string.Format(Environment.NewLine + LocalizedStrings.TraceLogger_PrintOneException_Format,
                level,
                exception.GetType(),
                message,
                stack);
        }

        #endregion

    }
}
