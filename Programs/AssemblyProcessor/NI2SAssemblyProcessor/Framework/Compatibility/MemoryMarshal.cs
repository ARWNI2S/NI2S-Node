#if NETSTANDARD2_0
using System;
using System.Runtime.CompilerServices;

namespace NI2S.Framework.Compatibility
{
    internal sealed class MemoryMarshal
    {
        /// <summary>
        /// Creates a new read-only span over a portion of a regular managed object. This can be useful
        /// if part of a managed object represents a "fixed array." This is dangerous because the
        /// <paramref name="length"/> is not checked.
        /// </summary>
        /// <param name="reference">A reference to data.</param>
        /// <param name="length">The number of <typeparamref name="T"/> elements the memory contains.</param>
        /// <returns>A read-only span representing the specified reference and length.</returns>
        /// <remarks>
        /// This method should be used with caution. It is dangerous because the length argument is not checked.
        /// Even though the ref is annotated as scoped, it will be stored into the returned span, and the lifetime
        /// of the returned span will not be validated for safety, even by span-aware languages.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ReadOnlySpan<T> CreateReadOnlySpan<T>(scoped ref T reference, int length) =>
            new ReadOnlySpan<T>(Unsafe.AsPointer(ref reference), length);
    }
}
#endif
