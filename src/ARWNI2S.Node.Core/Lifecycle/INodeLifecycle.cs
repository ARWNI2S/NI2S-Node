using ARWNI2S.Infrastructure.Lifecycle;

namespace ARWNI2S.Node.Core.Lifecycle
{
    /// <summary>
    /// The observable silo lifecycle.
    /// </summary>
    /// <remarks>
    /// This type is usually used as the generic parameter in <see cref="ILifecycleParticipant{INodeLifecycle}"/> as
    /// a means of participating in the lifecycle stages of a silo.
    /// </remarks>
    /// <seealso cref="ILifecycleObservable" />
    public interface INodeLifecycle : ILifecycleObservable
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
