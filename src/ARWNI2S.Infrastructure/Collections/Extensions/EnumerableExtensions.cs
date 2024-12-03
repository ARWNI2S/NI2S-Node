using ARWNI2S.Collections.Internals;
using System.Collections;

namespace ARWNI2S.Extensions
{
    public static class EnumerableExtensions
    {

        /// <summary>
        /// Given a non-generic IEnumerable interface, wrap a generic IEnumerable&lt;T&gt;
        /// interface around it. The generic interface will enumerate the same objects as the 
        /// underlying non-generic collection, but can be used in places that require a generic interface.
        /// The underlying non-generic collection must contain only items that
        /// are of type <typeparamref name="T"/> or a type derived from it. This method is useful
        /// when interfacing older, non-generic collections to newer code that uses generic interfaces.
        /// </summary>
        /// <remarks>Some collections implement both generic and non-generic interfaces. For efficiency,
        /// this method will first attempt to cast <paramref name="untypedCollection"/> to IEnumerable&lt;T&gt;. 
        /// If that succeeds, it is returned; otherwise, a wrapper object is created.</remarks>
        /// <typeparam name="T">The item type of the wrapper collection.</typeparam>
        /// <param name="untypedCollection">An untyped collection. This collection should only contain
        /// items of type <typeparamref name="T"/> or a type derived from it. </param>
        /// <returns>A generic IEnumerable&lt;T&gt; wrapper around <paramref name="untypedCollection"/>. 
        /// If <paramref name="untypedCollection"/> is null, then null is returned.</returns>
        public static IEnumerable<T> TypedAs<T>(this IEnumerable untypedCollection)
        {
            if (untypedCollection == null)
                return null;
            else if (untypedCollection is IEnumerable<T>)
                return (IEnumerable<T>)untypedCollection;
            else
                return new TypedEnumerable<T>(untypedCollection);
        }
    }
}
