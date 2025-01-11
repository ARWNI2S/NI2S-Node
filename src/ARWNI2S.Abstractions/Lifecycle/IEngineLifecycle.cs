// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Orleans;

namespace ARWNI2S.Lifecycle
{
    /// <summary>
    /// The observable silo lifecycle.
    /// </summary>
    /// <remarks>
    /// This type is usually used as the generic parameter in <see cref="ILifecycleParticipant{ISiloLifecycle}"/> as
    /// a means of participating in the lifecycle stages of a silo.
    /// </remarks>
    /// <seealso cref="ILifecycleObservable" />
    public interface IEngineLifecycle : ILifecycleObservable
    {
        /// <summary>
        /// The highest lifecycle stage which has completed starting.
        /// </summary>
        int HighestCompletedStage { get; }

        /// <summary>
        /// The lowest lifecycle stage which has completed stopping.
        /// </summary>
        int LowestStoppedStage { get; }
    }
}
