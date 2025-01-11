// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

#nullable enable
using ARWNI2S.Engine.Core;
using System.Buffers;

namespace ARWNI2S.Lifecycle
{
    /// <summary>
    /// The observable actor lifecycle.
    /// </summary>
    /// <remarks>
    /// This type is usually used as the generic parameter in <see cref="Orleans.ILifecycleParticipant{IActorLifecycle}"/> as
    /// a means of participating in the lifecycle stages of a actor activation.
    /// </remarks>
    public interface IActorLifecycle : Orleans.ILifecycleObservable
    {
        /// <summary>
        /// Registers a actor transition participant.
        /// </summary>
        /// <param name="participant">The participant.</param>
        void AddTransitionParticipant(IActorTransitionParticipant participant);

        /// <summary>
        /// Unregisters a actor transition participant.
        /// </summary>
        /// <param name="participant">The participant.</param>
        void RemoveTransitionParticipant(IActorTransitionParticipant participant);
    }

    public interface IActorTransitionParticipant
    {
        /// <summary>
        /// Called on the original activation when transition is initiated, after <see cref="INiisActor.OnDeactivateAsync(DeactivationReason, CancellationToken)"/> completes.
        /// The participant can access and update the deflation context.
        /// </summary>
        /// <param name="deflationContext">The deflation context.</param>
        void OnDehydrate(IDeflationContext deflationContext);

        /// <summary>
        /// Called on the new activation after a transition, before <see cref="INiisActor.OnActivateAsync(CancellationToken)"/> is called.
        /// The participant can restore state from the transition context.
        /// </summary>
        /// <param name="inflationContext">The inflation context.</param>
        void OnRehydrate(IInflationContext inflationContext);
    }

    /// <summary>
    /// Records the state of a actor activation which is in the process of being dehydrated for transition to another location.
    /// </summary>
    public interface IDeflationContext
    {
        /// <summary>
        /// Gets the keys in the context.
        /// </summary>
        IEnumerable<string> Keys { get; }

        /// <summary>
        /// Adds a sequence of bytes to the deflation context, associating the sequence with the provided key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        void AddBytes(string key, ReadOnlySpan<byte> value);

        /// <summary>
        /// Adds a sequence of bytes to the deflation context, associating the sequence with the provided key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="valueWriter">A delegate used to write the provided value to the context.</param>
        /// <param name="value">The value to provide to <paramref name="valueWriter"/>.</param>
        void AddBytes<T>(string key, Action<T, IBufferWriter<byte>> valueWriter, T value);

        /// <summary>
        /// Attempts to a value to the deflation context, associated with the provided key, serializing it using <see cref="Orleans.Serialization.Serializer"/>.
        /// If a serializer is found for the value, and the key has not already been added, then the value is added and the method returns <see langword="true"/>.
        /// If no serializer exists or the key has already been added, then the value is not added and the method returns <see langword="false"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value to add.</param>
        bool TryAddValue<T>(string key, T? value);
    }

    /// <summary>
    /// Contains the state of a actor activation which is in the process of being rehydrated after moving from another location.
    /// </summary>
    public interface IInflationContext
    {
        /// <summary>
        /// Gets the keys in the context.
        /// </summary>
        IEnumerable<string> Keys { get; }

        /// <summary>
        /// Tries to get a sequence of bytes from the inflation context, associated with the provided key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value, if present.</param>
        /// <returns><see langword="true"/> if the key exists in the context, otherwise <see langword="false"/>.</returns>
        bool TryGetBytes(string key, out ReadOnlySequence<byte> value);

        /// <summary>
        /// Tries to get a value from the inflation context, associated with the provided key, deserializing it using <see cref="Orleans.Serialization.Serializer"/>.
        /// If a serializer is found for the value, and the key is present, then the value is deserialized and the method returns <see langword="true"/>.
        /// If no serializer exists or the key has already been added, then the value is not added and the method returns <see langword="false"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value, if present.</param>
        /// <returns><see langword="true"/> if the key exists in the context, otherwise <see langword="false"/>.</returns>
        bool TryGetValue<T>(string key, out T? value);
    }
}
