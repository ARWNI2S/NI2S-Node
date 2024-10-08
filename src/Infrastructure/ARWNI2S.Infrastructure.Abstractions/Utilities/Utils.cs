using ARWNI2S.Infrastructure.Logging;
using ARWNI2S.Infrastructure.Resources;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace ARWNI2S.Infrastructure.Utilities
{
    /// <summary>
    /// The Utils class contains a variety of utility methods for use in application and entity code.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Returns a human-readable text string that describes an IEnumerable collection of objects.
        /// </summary>
        /// <typeparam name="T">The type of the list elements.</typeparam>
        /// <param name="collection">The IEnumerable to describe.</param>
        /// <param name="toString"></param>
        /// <param name="separator"></param>
        /// <param name="putInBrackets"></param>
        /// <returns>A string assembled by wrapping the string descriptions of the individual
        /// elements with square brackets and separating them with commas.</returns>
        public static string EnumerableToString<T>(IEnumerable<T> collection, Func<T, string> toString = null,
                                                        string separator = ", ", bool putInBrackets = true)
        {
            if (collection == null)
            {
                if (putInBrackets) return "[]";
                else return "null";
            }
            var sb = new StringBuilder();
            if (putInBrackets) sb.Append("[");
            var enumerator = collection.GetEnumerator();
            bool firstDone = false;
            while (enumerator.MoveNext())
            {
                T value = enumerator.Current;
                string val;
                if (toString != null)
                    val = toString(value);
                else
                    val = value == null ? "null" : value.ToString();

                if (firstDone)
                {
                    sb.Append(separator);
                    sb.Append(val);
                }
                else
                {
                    sb.Append(val);
                    firstDone = true;
                }
            }
            if (putInBrackets) sb.Append("]");
            return sb.ToString();
        }

        /// <summary>
        /// Returns a human-readable text string that describes a dictionary that maps objects to objects.
        /// </summary>
        /// <typeparam name="T1">The type of the dictionary keys.</typeparam>
        /// <typeparam name="T2">The type of the dictionary elements.</typeparam>
        /// <param name="dict">The dictionary to describe.</param>
        /// <param name="toString"></param>
        /// <param name="separator">Whether the elements should appear separated by </param>
        /// <returns>A string assembled by wrapping the string descriptions of the individual
        /// pairs with square brackets and separating them with commas.
        /// Each key-value pair is represented as the string description of the key followed by
        /// the string description of the value,
        /// separated by " -> ", and enclosed in curly brackets.</returns>
        public static string DictionaryToString<T1, T2>(ICollection<KeyValuePair<T1, T2>> dict, Func<T2, string> toString = null, string separator = null)
        {
            if (dict == null || dict.Count == 0)
            {
                return "[]";
            }
            if (separator == null)
            {
                separator = Environment.NewLine;
            }
            var sb = new StringBuilder("[");
            var enumerator = dict.GetEnumerator();
            int index = 0;
            while (enumerator.MoveNext())
            {
                var pair = enumerator.Current;
                sb.Append("{");
                sb.Append(pair.Key);
                sb.Append(" -> ");

                string val;
                if (toString != null)
                    val = toString(pair.Value);
                else
                    val = pair.Value == null ? "null" : pair.Value.ToString();
                sb.Append(val);

                sb.Append("}");
                if (index++ < dict.Count - 1)
                    sb.Append(separator);
            }
            sb.Append("]");
            return sb.ToString();
        }

        public static string MapPath(string path)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(baseDirectory, path);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string TimeSpanToString(TimeSpan timeSpan)
        {
            //00:03:32.8289777
            return String.Format("{0}h:{1}m:{2}s.{3}ms", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        }

        /// <summary>
        /// 
        /// </summary>
        public static long TicksToMilliSeconds(long ticks)
        {
            return (long)TimeSpan.FromTicks(ticks).TotalMilliseconds;
        }

        /// <summary>
        /// 
        /// </summary>
        public static float AverageTicksToMilliSeconds(float ticks)
        {
            return (float)TimeSpan.FromTicks((long)ticks).TotalMilliseconds;
        }

        /// <summary>
        /// Calculates an integer hash value based on the consistent identity hash of a string.
        /// </summary>
        /// <param name="text">The string to hash.</param>
        /// <returns>An integer hash for the string.</returns>
        public static int CalculateIdHash(string text)
        {
            SHA256 sha = SHA256.Create(); // This is one implementation of the abstract class SHA1.
            int hash = 0;
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(text);
                byte[] result = sha.ComputeHash(data);
                for (int i = 0; i < result.Length; i += 4)
                {
                    int tmp = (result[i] << 24) | (result[i + 1] << 16) | (result[i + 2] << 8) | (result[i + 3]);
                    hash = hash ^ tmp;
                }
            }
            finally
            {
                sha.Dispose();
            }
            return hash;
        }

        /// <summary>
        /// Calculates a Guid hash value based on the consistent identity a string.
        /// </summary>
        /// <param name="text">The string to hash.</param>
        /// <returns>An integer hash for the string.</returns>
        public static Guid CalculateGuidHash(string text)
        {
            SHA256 sha = SHA256.Create(); // This is one implementation of the abstract class SHA1.
            byte[] hash = new byte[16];
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(text);
                byte[] result = sha.ComputeHash(data);
                for (int i = 0; i < result.Length; i++)
                {
                    byte tmp = (byte)(hash[i % 16] ^ result[i]);
                    hash[i % 16] = tmp;
                }
            }
            finally
            {
                sha.Dispose();
            }
            return new Guid(hash);
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool TryFindException(Exception original, Type targetType, out Exception target)
        {
            if (original.GetType() == targetType)
            {
                target = original;
                return true;
            }
            else if (original is AggregateException)
            {
                var baseEx = original.GetBaseException();
                if (baseEx.GetType() == targetType)
                {
                    target = baseEx;
                    return true;
                }
                else
                {
                    var newEx = ((AggregateException)original).Flatten();
                    foreach (var exc in newEx.InnerExceptions)
                    {
                        if (exc.GetType() == targetType)
                        {
                            target = newEx;
                            return true;
                        }
                    }
                }
            }
            target = null;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void SafeExecute(Action action, Logger logger = null, string caller = null)
        {
            SafeExecute(action, logger, caller == null ? (Func<string>)null : () => caller);
        }

        /// <summary>
        /// a function to safely execute an action without any exception being thrown.
        /// callerGetter function is called only in faulty case (now string is generated in the success case).
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void SafeExecute(Action action, Logger logger, Func<string> callerGetter)
        {
            try
            {
                action();
            }
            catch (Exception exc)
            {
                try
                {
                    if (logger != null)
                    {
                        string caller = null;
                        if (callerGetter != null)
                        {
                            try
                            {
                                caller = callerGetter();
                            }
                            catch (Exception) { }
                        }
                        foreach (var e in exc.FlattenAggregate())
                        {
                            logger.Warn((int)TraceCode.NodeRuntime_Error_100325, String.Format(ErrorStrings.NodeRuntime_Error_100325, e.GetType().FullName, caller ?? string.Empty), exc);
                        }
                    }
                }
                catch (Exception)
                {
                    // now really, really ignore.
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static TimeSpan Since(DateTime start)
        {
            return DateTime.UtcNow.Subtract(start);
        }

        /// <summary>
        /// 
        /// </summary>
        public static AggregateException Flatten(this ReflectionTypeLoadException rtle)
        {
            // if ReflectionTypeLoadException is thrown, we need to provide the
            // LoaderExceptions property in order to make it meaningful.
            var all = new List<Exception> { rtle };
            all.AddRange(rtle.LoaderExceptions);
            throw new AggregateException(ErrorStrings.Utils_Flatten_AggregateException_Message, all);
        }

        public static MethodInfo GetStaticMethodThroughReflection(string assemblyName, string className, string methodName, Type[] argumentTypes)
        {
            var asm = Assembly.Load(new AssemblyName(assemblyName));
            if (asm == null)
                throw new InvalidOperationException(string.Format(ErrorStrings.Utils_AssemblyNull_Format, assemblyName));

            var cl = asm.GetType(className);
            if (cl == null)
                throw new InvalidOperationException(string.Format(ErrorStrings.Utils_ClassNull_Format, className, assemblyName));

            MethodInfo method;
            method = argumentTypes == null
                ? cl.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).Where(m => m.Name == methodName).FirstOrDefault()
                : cl.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, argumentTypes, null);

            if (method == null)
                throw new InvalidOperationException(string.Format(ErrorStrings.Utils_GetStaticMethodThroughReflection_MethodNull_Format, methodName, className, assemblyName));

            return method;
        }

        public static object InvokeStaticMethodThroughReflection(string assemblyName, string className, string methodName, Type[] argumentTypes, object[] arguments)
        {
            var method = GetStaticMethodThroughReflection(assemblyName, className, methodName, argumentTypes);
            return method.Invoke(null, arguments);
        }

        public static Type LoadTypeThroughReflection(string assemblyName, string className)
        {
            var asm = Assembly.Load(new AssemblyName(assemblyName));
            if (asm == null) throw new InvalidOperationException(string.Format(ErrorStrings.Utils_AssemblyNull_Format, assemblyName));

            var cl = asm.GetType(className);
            if (cl == null) throw new InvalidOperationException(string.Format(ErrorStrings.Utils_ClassNull_Format, className, assemblyName));

            return cl;
        }

        public static List<T> Union<T>(List<T> list, List<T> other)
        {
            if (list == null && other == null)
                return null;
            if (list == null)
                return other;
            if (other == null)
                return list;
            list.AddRange(other);
            return list;
        }
    }
}
